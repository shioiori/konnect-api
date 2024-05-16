using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UTCClassSupport.API.Authorize.Requests;
using UTCClassSupport.API.Authorize.Responses;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Utilities;

namespace UTCClassSupport.API.Authorize
{
  [ApiController]
  public class AccessController : ControllerBase
  {
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly EFContext _dbContext;

    public AccessController(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IMapper mapper,
        IConfiguration configuration,
        EFContext dbContext)
    {
      _userManager = userManager;
      _roleManager = roleManager;
      _mapper = mapper;
      _configuration = configuration;
      _dbContext = dbContext;
    }

    [HttpPost("login")]
    public async Task<Response> LoginAsync(LoginRequest request)
    {
      var user = await _userManager.FindByNameAsync(request.Username);
      if (user != null)
      {
        if (await _userManager.CheckPasswordAsync(user, request.Password))
        {
          var claims = new List<Claim>();
          claims.Add(new Claim(ClaimData.UserID, user.Id));
          claims.Add(new Claim(ClaimData.UserName, user.UserName));
          claims.Add(new Claim(ClaimData.DisplayName, user.DisplayName));
          claims.Add(new Claim(ClaimData.Email, user.Email));
          claims.Add(new Claim(ClaimData.Avatar, user.Avatar));
          claims.Add(new Claim(ClaimData.Tel, user.PhoneNumber ?? String.Empty));
          claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
          var token = GetToken(claims);

          return new AuthenticationResponse()
          {
            Success = true,
            Message = "Login success",
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            StatusCode = StatusCodes.Status200OK,
          };
        }
        else
        {
          return new AuthenticationResponse()
          {
            Success = false,
            Message = "Username or password is not match",
            StatusCode = StatusCodes.Status401Unauthorized
          };
        }
      }
      return new AuthenticationResponse()
      {
        Success = false,
        Message = "Username or password is not match",
        StatusCode = StatusCodes.Status401Unauthorized
      };
    }

    [HttpPost("login/{groupId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<Response> LoginWithGroup(string groupId)
    {
      var identity = HttpContext.User.Identity as ClaimsIdentity;
      var userId = identity.FindFirst(ClaimData.UserID).Value;
      var userName = identity.FindFirst(ClaimData.UserName).Value;
      var data = _dbContext.UserGroupRoles.FirstOrDefault(x => x.UserId == userId && x.GroupId == groupId);
      if (data == default)
      {
        return new AuthenticationResponse()
        {
          Success = false,
          Message = "Group is not valid",
          StatusCode = StatusCodes.Status400BadRequest
        };
      }
      var claims = identity.Claims.ToList();
      claims.Add(new Claim(ClaimData.GroupID, data.GroupId));
      var role = await _roleManager.FindByIdAsync(data.RoleId);
      claims.Add(new Claim(ClaimData.RoleID, data.RoleId));
      claims.Add(new Claim(ClaimData.RoleName, role.Name));
      claims.Add(new Claim(ClaimTypes.Role, role.Name));
      var token = GetToken(claims);

      return new AuthenticationResponse()
      {
        Success = true,
        Message = "Redirect to group",
        AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
        StatusCode = StatusCodes.Status200OK,
      };
    }

    [HttpPost("register")]
    public async Task<Response> RegisterAsync(RegisterRequest request, string? groupId = "")
    {
      GroupRole groupRole = GroupRole.Manager;
      if (_dbContext.Groups.FirstOrDefault(x => x.Id == groupId) != default)
      {
        groupRole = GroupRole.User;
      }
      var userExists = await _userManager.FindByNameAsync(request.UserName);
      if (userExists != null)
      {
        return new AuthenticationResponse()
        {
          Success = false,
          Message = "This account is registed",
          StatusCode = StatusCodes.Status500InternalServerError
        };
      }
      var user = _mapper.Map<RegisterRequest, User>(request);
      var result = await _userManager.CreateAsync(user, request.Password);
      if (!result.Succeeded)
      {
        return new AuthenticationResponse()
        {
          Success = false,
          Message = "Create user fail",
          StatusCode = StatusCodes.Status500InternalServerError
        };
      }
      if (!await _roleManager.RoleExistsAsync(groupRole.ToString()))
      {
        await _roleManager.CreateAsync(new Role()
        {
          Name = groupRole.ToString(),
        });
      }
      await _userManager.AddToRoleAsync(user, groupRole.ToString());
      if (string.IsNullOrEmpty(groupId))
      {
        var group = new Group()
        {
          Name = Guid.NewGuid().ToString(),
        };
        _dbContext.Groups.Add(group);
        _dbContext.SaveChanges();
        groupId = group.Id;
      }
      var role = await _roleManager.FindByNameAsync(groupRole.ToString());
      _dbContext.UserGroupRoles.Add(new UserGroupRole()
      {
        UserId = user.Id,
        GroupId = groupId,
        RoleId = role.Id
      }); 
      _dbContext.SaveChanges();
      return new AuthenticationResponse()
      {
        Success = true,
        Message = "Register success",
        StatusCode = StatusCodes.Status200OK,
       };
    }

    [HttpPost("forgot/password")]
    public async Task<Response> ForgotPassword()
    {
      throw new NotImplementedException();
    }

    [HttpPost("join/{token}")]
    public async Task<Response> JoinByGroup(string token)
    {
      var key = AesOperation.GetKey(token);
      var encryptText = AesOperation.GetEncryptText(token);
      var decryptText = AesOperation.DecryptString(key, encryptText);
      var request = JsonConvert.DeserializeObject<JoinRequest>(decryptText);
      return new JoinGroupResponse()
      {
        Message = "Thực hiện đăng ký trước khi vào group",
        Success = true,
        Type = ResponseType.Success,
        GroupId = request.GroupId,
        Email = request.Email,
      };
    }


    [NonAction]
    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
      var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

      var token = new JwtSecurityToken(
          issuer: _configuration["JWT:Issuer"],
          audience: _configuration["JWT:Issuer"],
          expires: DateTime.Now.AddHours(3),
          claims: authClaims,
          signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
      );
      return token;
    }
  }
}
