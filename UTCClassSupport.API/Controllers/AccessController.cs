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
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Infrustructure.Repositories;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Utilities;

namespace UTCClassSupport.API.Authorize
{
  [ApiController]
  public class AccessController : ControllerBase
  {
    private readonly AccessManager _accessManager;
    public AccessController(
        AccessManager accessManager)
    {
      _accessManager = accessManager;
    }

    [HttpPost("login")]
    public Task<Response> LoginAsync(LoginRequest request)
    {
      return _accessManager.GetLoginToken(request);
    }

    [HttpPost("login/{groupId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public Task<Response> LoginWithGroup(string groupId)
    {
      var identity = HttpContext.User.Identity as ClaimsIdentity;
      return _accessManager.GetLoginWithGroupToken(identity, groupId);
    }

    [HttpPost("register")]
    public Task<Response> RegisterAsync([FromBody]RegisterRequest request, string? groupId = "")
    {
      return _accessManager.RegisterAsync(request, groupId);
    }

    [HttpPost("password/forgot")]
    public async Task<Response> ForgotPassword(string email)
    {
      var valid = await _accessManager.GenerateForgotPasswordMailAsync(email);
      if (!valid)
      {
        return new Response()
        {
          Success = false,
          Type = ResponseType.Error,
          Message = "Có vẻ bạn chưa đăng ký tài khoản"
        };
      }
      return new Response()
      {
        Success = true,
        Type = ResponseType.Success,
        Message = "Một tin nhắn chứa mật khẩu mới vừa được gửi tới email của bạn."
      };
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
  }
}
