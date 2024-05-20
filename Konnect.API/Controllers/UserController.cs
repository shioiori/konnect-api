using Konnect.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Infrustructure.Repositories;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Responses.DTOs;

namespace UTCClassSupport.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
  [Route("user")]
  public class UserController : BaseController
  {
    private readonly UserRepository _userRepository;
    public UserController(UserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    [HttpGet]
    public UserInfo GetUserData()
    {
      return ReadJWTToken();
    }

    [HttpGet("/{username}")]
    public async Task<Response> GetUserDataAsync(string username)
    {
      var data = await _userRepository.GetUserAsync(username);
      return new GetUserResponse()
      {
        Success = true,
        Type = ResponseType.Success,
        User = data,
      };
    }

    [HttpGet("group")]
    public Response GetUsers(string? groupId)
    {
      var userData = ReadJWTToken();
      if (groupId == null)
      {
        groupId = userData.GroupId;
      }
      var data = _userRepository.GetUsers(groupId);
      return new GetUsersResponse()
      {
        Success = true,
        Type = ResponseType.Success,
        Users = data,
      };
    }

    [HttpPost]
    public async Task<Response> AddUserAsync([FromBody] AddUserRequest request)
    {
      var userData = ReadJWTToken();
      if (request.UserGroupData == null)
      {
        request.UserGroupData = new UserGroupData()
        {
          UserName = request.UserName,
          GroupId = userData.GroupId,
          RoleName = GroupRole.User.ToString(),
        };
      }
      var data = await _userRepository.AddUserAsync(request);
      return new AddUserResponse()
      {
        Success = true,
        Message = "Thêm người dùng thành công",
        Type = ResponseType.Success,
        User = data
      };
    }

    [HttpPost("{username}")]
    public async Task<Response> EditUserAsync(string username, [FromBody] UpdateUserRequest request)
    {
      var data = await _userRepository.UpdateUserAsync(username, request);
      return new UpdateUserResponse()
      {
        Success = true,
        Message = "Cập nhật người dùng thành công",
        Type = ResponseType.Success,
        User = data
      };
    }

    [HttpDelete("{username}")]
    public async Task<Response> DeleteUserAsync(string username)
    {
      try
      {
        _userRepository.DeleteUserAsync(username);
        return new DeleteUserResponse()
        {
          Success = true,
          Message = "Xóa người dùng thành công",
          Type = ResponseType.Success,
        };
      }
      catch (Exception ex)
      {
        return new DeleteUserResponse()
        {
          Success = false,
          Type = ResponseType.Error,
          Message = ex.Message,
        };
      }
    }

    [HttpPost("{groupId?}/{username}/{role}")]
    public async Task<Response> ChangeRole(string userName, string role, string? groupId)
    {
      try
      {
        var userData = ReadJWTToken();
        if (groupId == null)
        {
          groupId = userData.GroupId;
        }
        _userRepository.ChangeRoleAsync(userName, role, groupId);
        return new DeleteUserResponse()
        {
          Success = true,
          Message = "Xóa người dùng thành công",
          Type = ResponseType.Success,
        };
      }
      catch (Exception ex)
      {
        return new DeleteUserResponse()
        {
          Success = false,
          Type = ResponseType.Error,
          Message = ex.Message,
        };
      }
    }

    [HttpGet("check-email-confirmed")]
    public async Task<bool> CheckEmailConfirmedAsync(string? email)
    {
      var userData = ReadJWTToken();
      if (email == null)
      {
        email = userData.Email;
      }
      return await _userRepository.IsEmailConfirmedAsync(email);
    }

    [HttpPost("password/change")]
    public async Task<Response> ChangePassword([FromBody] ChangePasswordRequest request)
    {
      var data = ReadJWTToken();
      var rightPassword = await _userRepository.CheckPassword(data.UserName, request.OldPassword);
      if (!rightPassword)
      {
        return new ChangePasswordResponse()
        {
          Success = false,
          Type = ResponseType.Error,
          Message = "Mật khẩu cũ không đúng"
        };
      }
      if (await _userRepository.ChangePasswordAsync(data.UserName, request.OldPassword, request.NewPassword))
      {
        return new ChangePasswordResponse()
        {
          Success = true,
          Type = ResponseType.Success,
          Message = "Đổi mật khẩu thành công"
        };
      }
      else
      {
        return new ChangePasswordResponse()
        {
          Success = false,
          Type = ResponseType.Error,
          Message = "Có lỗi trong quá trình thay đổi mật khẩu"
        };
      }
    }
  }
}
