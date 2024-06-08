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
using UTCClassSupport.API.Requests.Pagination;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Responses.DTOs;

namespace UTCClassSupport.API.Controllers
{
	[Authorize(AuthenticationSchemes = "Bearer")]
	[Route("user")]
	public class UserController : BaseController
	{
		private readonly IUserRepository _userRepository;
		private readonly NotificationManager _notificationManager;
		public UserController(IUserRepository userRepository, NotificationManager notificationManager)
		{
			_userRepository = userRepository;
			_notificationManager = notificationManager;
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
		public Response GetUsers(string? groupId, [FromQuery] PaginationData? pagination)
		{
			var userData = ReadJWTToken();
			if (groupId == null)
			{
				groupId = userData.GroupId;
			}
			var data = _userRepository.GetUsers(groupId, pagination).OrderBy(x => x.UserName).ToList();
			int total = _userRepository.GetTotalUser(groupId);

			if (pagination == null)
			{
				pagination = new PaginationData()
				{
					PageNumber = 1,
					PageSize = total
				};
			}
			return new GetUsersResponse()
			{
				Success = true,
				Type = ResponseType.Success,
				Users = data,
				Total = _userRepository.GetTotalUser(groupId),
				Pagination = pagination
			};
		}

		/// <summary>
		/// Add user to group
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
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
			return await _userRepository.AddUserAsync(request);
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

		/// <summary>
		/// Delete account
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		[HttpDelete("{username}")]
		public async Task<Response> DeleteUserAsync(string username)
		{
			try
			{
				await _userRepository.DeleteUserAsync(username);
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

		[HttpDelete("{username}/kick")]
		public async Task<Response> KickUserFromGroup(string username)
		{
			try
			{
				var userData = ReadJWTToken();
				return await _userRepository.KickUserFromGroupAsync(username, userData.GroupId, userData.UserName);
			}
			catch (Exception ex)
			{
				return new Response()
				{
					Success = false,
					Type = ResponseType.Error,
					Message = ex.Message,
				};
			}
		}

		[HttpPost("{userName}/{role}")]
		public async Task<Response> ChangeRole(string userName, string role)
		{
			try
			{
				var userData = ReadJWTToken();
				await _userRepository.ChangeRoleAsync(userName, role, userData.GroupId);
				return new Response()
				{
					Success = true,
					Message = "Thay đổi role thành công",
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
