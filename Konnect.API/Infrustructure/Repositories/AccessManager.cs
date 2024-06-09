using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Common.Mail;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Utilities;

namespace UTCClassSupport.API.Infrustructure.Repositories
{
  public class AccessManager
  {
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly EFContext _dbContext;
    private readonly MailSettings _mailSettings;

    public AccessManager(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IConfiguration configuration,
        IOptions<MailSettings> mailSettings,
        EFContext dbContext)
    {
      _userManager = userManager;
      _roleManager = roleManager;
      _configuration = configuration;
      _dbContext = dbContext;
      _mailSettings = mailSettings.Value;
    }
    public async Task<Response> GetLoginWithGroupToken(ClaimsIdentity identity, string groupId)
    {
      var userId = identity.FindFirst(ClaimData.UserID).Value;
      var userName = identity.FindFirst(ClaimData.UserName).Value;
      var request = await GetLoginToken(new LoginRequest()
      {
        Username = userName,
        IsLogin = true,
      });
      var token = new JwtSecurityTokenHandler().ReadJwtToken(((AuthenticationResponse)request).AccessToken);
      var data = _dbContext.UserGroupRoles.FirstOrDefault(x => x.UserId == userId && x.GroupId == groupId);
      if (data == default)
      {
        return new AuthenticationResponse()
        {
          Success = false,
          Message = "Không tồn tại group này",
          StatusCode = StatusCodes.Status400BadRequest
        };
      }
      var claims = token.Claims.ToList();
      claims.Add(new Claim(ClaimData.GroupID, data.GroupId));
      var role = await _roleManager.FindByIdAsync(data.RoleId);
      claims.Add(new Claim(ClaimData.RoleID, data.RoleId));
      claims.Add(new Claim(ClaimData.RoleName, role.Name));
      claims.Add(new Claim(ClaimTypes.Role, role.Name));
      var newToken = GetToken(claims);

      return new AuthenticationResponse()
      {
        Success = true,
        Message = "Dịch chuyển tới group " + groupId + "...",
        AccessToken = new JwtSecurityTokenHandler().WriteToken(newToken),
        StatusCode = StatusCodes.Status200OK,
      };
    }

    public async Task<Response> GetLoginToken(LoginRequest request)
    {
      var user = await _userManager.FindByNameAsync(request.Username);
      if (user != null)
      {
        if (request.IsLogin || await _userManager.CheckPasswordAsync(user, request.Password))
        {
          var claims = new List<Claim>();
          claims.Add(new Claim(ClaimData.UserID, user.Id));
          claims.Add(new Claim(ClaimData.UserName, user.UserName));
          claims.Add(new Claim(ClaimData.DisplayName, user.DisplayName));
          claims.Add(new Claim(ClaimData.Email, user.Email));
          claims.Add(new Claim(ClaimData.Avatar, user.Avatar));
          claims.Add(new Claim(ClaimData.PhoneNumber, user.PhoneNumber ?? String.Empty));
          claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
          var token = GetToken(claims);

          return new AuthenticationResponse()
          {
            Success = true,
            Type = ResponseType.Success,
            Message = "Đăng nhập thành công",
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            StatusCode = StatusCodes.Status200OK,
          };
        }
        else
        {
          return new AuthenticationResponse()
          {
            Success = false,
            Type = ResponseType.Error,
            Message = "Username hoặc mật khẩu không khớp",
            StatusCode = StatusCodes.Status401Unauthorized
          };
        }
      }
      return new AuthenticationResponse()
      {
        Success = false,
        Type = ResponseType.Error,
        Message = "Username hoặc mật khẩu không khớp",
        StatusCode = StatusCodes.Status401Unauthorized
      };
    }

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
          Type = ResponseType.Error,
          Message = "Tài khoản này đã được đăng ký",
          StatusCode = StatusCodes.Status500InternalServerError
        };
      }
      var user = CustomMapper.Mapper.Map<RegisterRequest, User>(request);
      var result = await _userManager.CreateAsync(user, request.Password);
      if (!result.Succeeded)
      {
        return new AuthenticationResponse()
        {
          Success = false,
          Type = ResponseType.Error,
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
        Message = "Đăng ký thành công",
        Type = ResponseType.Success,
        StatusCode = StatusCodes.Status200OK,
      };
    }

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

    public async Task<bool> GenerateForgotPasswordMailAsync(string email)
    {
      try
      {
        var user = _userManager.Users.FirstOrDefault(x => x.Email == email);
        if (user == default)
        {
          return false;
        }
        var newPassword = StringHelper.GenerateString();
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        _userManager.ResetPasswordAsync(user, token, newPassword);
        _dbContext.SaveChangesAsync();
        MailHandler mailHandler = new MailHandler(_mailSettings);
        mailHandler.Send(new MailContent()
        {
          To = email,
          Subject = GetForgotPasswordMailSubject(user.DisplayName),
          Body = GetForgotPasswordMailContent(user.DisplayName, newPassword)
        });
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    private string GetForgotPasswordMailSubject(string displayName)
    {
      return "Mật khẩu của bạn đã được cập nhật lại";
    }

    private string GetForgotPasswordMailContent(string displayName, string newPassword)
    {
      return $"Thân gửi {displayName}, mật khẩu của bạn đã được cập nhật lại. Hiện tại mật khẩu của bạn là <br/><h4>{newPassword}</h4><br/>Sau khi đăng nhập, bạn có thể vào lại trang cá nhân để thay đổi mật khẩu.";
    }
  }
}
