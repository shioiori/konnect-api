using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UTCClassSupport.API.Authorize.Requests;
using UTCClassSupport.API.Authorize.Responses;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;

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

    [HttpGet("login")]
    public async Task<AuthenticationResponse> LoginAsync(LoginRequest request)
    {
      var user = await _userManager.FindByNameAsync(request.Username);
      if (user != null)
      {
        if (user.EmailConfirmed == false)
        {
          return new AuthenticationResponse()
          {
            Success = false,
            Message = "Email is not confirmed",
            StatusCode = StatusCodes.Status200OK,
          };
        }
        else if (await _userManager.CheckPasswordAsync(user, request.Password))
        {
          var userRoles = await _userManager.GetRolesAsync(user);
          var claims = new List<Claim>();
          claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
          claims.Add(new Claim(ClaimTypes.Name, user.UserName));
          claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
          foreach (var userRole in userRoles)
          {
            claims.Add(new Claim(ClaimTypes.Role, userRole));
          }

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

    [HttpPost("register/{id}")]
    public async Task<AuthenticationResponse> RegisterByGroupAsync(string id, RegisterRequest request)
    {
      //check if group id has in db
      if (_dbContext.Groups.FirstOrDefault(x => x.Id == id) != default)
      {
        return await RegisterAsync(request, id, GroupRole.User);
      }
      return new AuthenticationResponse()
      {
        Success = false,
        Message = "Group id not valid",
        StatusCode = StatusCodes.Status400BadRequest
      };
    }

    [HttpPost("register")]
    public async Task<AuthenticationResponse> RegisterAsync(RegisterRequest request, string groupId = "",
      GroupRole role = GroupRole.Manager)
    {
      var userExists = await _userManager.FindByNameAsync(request.Username);
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
      if (!await _roleManager.RoleExistsAsync(role.ToString()))
      {
        await _roleManager.CreateAsync(new Role()
        {
          Name = role.ToString(),
        });
      }
      if (!await _roleManager.RoleExistsAsync(role.ToString()))
      {
        await _roleManager.CreateAsync(new Role()
        {
          Name = role.ToString(),
        });
      }
      await _userManager.AddToRoleAsync(user, role.ToString());
      var group = new Group()
      {
        Name = Guid.NewGuid().ToString(),
      };
      if (string.IsNullOrEmpty(groupId))
      {
        _dbContext.Groups.Add(group);
        groupId = group.Id;
      }
      _dbContext.UserGroupRoles.Add(new UserGroupRole()
      {
        UserId = user.Id,
        GroupId = groupId,
        RoleId = (await _roleManager.FindByNameAsync(role.ToString())).Id
      });
      if (!result.Succeeded)
      {
        return new AuthenticationResponse()
        {
          Success = false,
          Message = "Create user fail",
          StatusCode = StatusCodes.Status500InternalServerError
        };
      }
      return new AuthenticationResponse()
      {
        Success = true,
        Message = "Register success",
        StatusCode = StatusCodes.Status200OK,
       };
    }
    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
      var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

      var token = new JwtSecurityToken(
          issuer: _configuration["JWT:Issuer"],
          audience: _configuration["JWT:Audience"],
          expires: DateTime.Now.AddHours(3),
          claims: authClaims,
          signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
      );
      return token;
    }
  }
}
