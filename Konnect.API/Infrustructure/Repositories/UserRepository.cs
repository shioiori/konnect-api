using DocumentFormat.OpenXml.Bibliography;
using Konnect.API.Infrustructure.Interfaces;
using Konnect.API.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Requests.Pagination;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Responses.DTOs;
using UTCClassSupport.API.Utilities;

namespace UTCClassSupport.API.Infrustructure.Repositories
{
	public interface IUserRepository
	{
		Task<Response> AddUserAsync(AddUserRequest request);
		Task<bool> ChangePasswordAsync(string username, string oldPassword, string newPassword);
		Task<bool> UpdateRoleAsync(string userName, string roleName, string groupId);
		Task<bool> CheckPassword(string username, string password);
		Task<bool> DeleteUserAsync(string userName);
		Task<UserDTO> GetUserAsync(string userName);
		Task<UserDTO> GetUserByEmailAsync(string email);
		List<UserDTO> GetUsers(string groupId);
		List<UserDTO> GetUsers(string groupId, PaginationData pagination, bool isPaging);
		Task<bool> IsEmailConfirmedAsync(string email);
		Task<bool> RemoveRoleFromGroupAsync(string userName, string groupId);
		Task<UserDTO> UpdateUserAsync(string userName, UpdateUserRequest request);
		Task<RoleDTO> GetGroupRoleAsync(string groupId, string userId);
		int GetTotalUser(string groupId);
		Task UpdateAvatar(string userName, string url);
	}

	public class UserRepository : IUserRepository
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;
		private readonly IConfiguration _configuration;
		private readonly EFContext _dbContext;

		public UserRepository(
			UserManager<User> userManager,
			RoleManager<Role> roleManager,
			IConfiguration configuration,
			EFContext dbContext)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
			_dbContext = dbContext;
		}

		public List<UserDTO> GetUsers(string groupId)
		{
			var user = _dbContext.UserGroupRoles.Where(x => x.GroupId == groupId)
								  .Include(x => x.User)
								  .Include(x => x.Role)
								  .Select(x => new UserDTO()
								  {
									  Id = x.UserId,
									  Email = x.User.Email,
									  PhoneNumber = x.User.PhoneNumber,
									  DisplayName = x.User.DisplayName,
									  UserName = x.User.UserName,
									  Avatar = x.User.Avatar,
									  RoleName = x.Role.Name,
									  GroupId = x.GroupId,
								  }).ToList();
			return user;
		}

		public async Task<UserDTO> GetUserAsync(string userName)
		{
			var user = await _userManager.FindByNameAsync(userName);
			return CustomMapper.Mapper.Map<UserDTO>(user);
		}

		public async Task<Response> AddUserAsync(AddUserRequest request)
		{
			try
			{
				if (await _userManager.FindByNameAsync(request.UserName) != default)
				{
					return new AddUserResponse()
					{
						Success = false,
						Type = ResponseType.Error,
						Message = "Người dùng này đã tồn tại",
					};
				}
				if (_userManager.Users.FirstOrDefault(x => x.Email == request.Email) != default)
				{
					return new AddUserResponse()
					{
						Success = false,
						Type = ResponseType.Error,
						Message = "Email này đã được đăng ký"
					};
				}
				var user = CustomMapper.Mapper.Map<User>(request);
				await _userManager.CreateAsync(user);
				await _userManager.AddPasswordAsync(user, request.UserName);
				_dbContext.SaveChanges();
				if (request.UserGroupData.RoleName == default)
				{
					request.UserGroupData.RoleName = GroupRole.User.ToString();
				}
				_dbContext.UserGroupRoles.Add(new UserGroupRole()
				{
					GroupId = request.UserGroupData.GroupId,
					UserId = user.Id,
					RoleId = (await _roleManager.FindByNameAsync(request.UserGroupData.RoleName)).Id
				});
				_dbContext.SaveChanges();
				var dto = CustomMapper.Mapper.Map<UserDTO>(user);
				dto.RoleName = request.UserGroupData.RoleName;
				dto.GroupId = request.UserGroupData.GroupId;
				return new AddUserResponse()
				{
					Success = true,
					Type = ResponseType.Success,
					Message = "Thêm người dùng thành công",
					User = dto
				};
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<UserDTO> UpdateUserAsync(string userName, UpdateUserRequest request)
		{
			var user = await _userManager.FindByNameAsync(userName);
			CustomMapper.Mapper.Map<UpdateUserRequest, User>(request, user);
			_dbContext.SaveChanges();
			var dto = CustomMapper.Mapper.Map<UserDTO>(user);
			return dto;
		}

		public async Task<bool> DeleteUserAsync(string userName)
		{
			try
			{
				var user = await _userManager.FindByNameAsync(userName);
				await _userManager.DeleteAsync(user);
				_dbContext.SaveChanges();
				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<bool> RemoveRoleFromGroupAsync(string userName, string groupId)
		{
			var user = await _userManager.FindByNameAsync(userName);
			var link = _dbContext.UserGroupRoles.Where(x => x.UserId == user.Id && x.GroupId == groupId);
			if (link == default) return false;
			_dbContext.UserGroupRoles.RemoveRange(link);
			_dbContext.SaveChanges();
			return true;
		}

		public async Task<bool> UpdateRoleAsync(string userName, string roleName, string groupId)
		{
			try
			{
				var role = await _roleManager.FindByNameAsync(roleName);
				var user = await _userManager.FindByNameAsync(userName);
				var link = _dbContext.UserGroupRoles.FirstOrDefault(x => x.UserId == user.Id && x.GroupId == groupId);
				if (link != null)
				{
					_dbContext.UserGroupRoles.Remove(link);
					_dbContext.SaveChanges();
				}
				var newLink = new UserGroupRole()
				{
					UserId = user.Id,
					GroupId = groupId,
					RoleId = role.Id
				};
				_dbContext.UserGroupRoles.Add(newLink);
				_dbContext.SaveChanges();
				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<bool> IsEmailConfirmedAsync(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			return await _userManager.IsEmailConfirmedAsync(user);
		}

		public async Task<bool> CheckPassword(string username, string password)
		{
			var user = await _userManager.FindByNameAsync(username);
			return await _userManager.CheckPasswordAsync(user, password);
		}

		public async Task<bool> ChangePasswordAsync(string username, string oldPassword, string newPassword)
		{
			var user = await _userManager.FindByNameAsync(username);
			var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
			_dbContext.SaveChanges();
			return result.Succeeded;
		}

		public List<UserDTO> GetUsers(string groupId, PaginationData pagination, bool isPaging)
		{
			if (!isPaging)
			{
				return GetUsers(groupId);
			}
			int skip = pagination.PageNumber * pagination.PageSize - pagination.PageSize;
			var user = _dbContext.UserGroupRoles.Where(x => x.GroupId == groupId)
								  .Include(x => x.User)
								  .Include(x => x.Role)
								  .Skip(skip).Take(pagination.PageSize)
								  .Select(x => new UserDTO()
								  {
									  Id = x.UserId,
									  Email = x.User.Email,
									  PhoneNumber = x.User.PhoneNumber,
									  DisplayName = x.User.DisplayName,
									  UserName = x.User.UserName,
									  Avatar = x.User.Avatar,
									  RoleName = x.Role.Name,
									  GroupId = x.GroupId,
								  }).ToList();
			return user;
		}

		public async Task<RoleDTO> GetGroupRoleAsync(string groupId, string userId)
		{
			var link = _dbContext.UserGroupRoles.FirstOrDefault(x => x.GroupId == groupId && x.UserId == userId);
			if (link == null)
			{
				return null;
			}
			var role = await _roleManager.FindByIdAsync(link.RoleId);
			return CustomMapper.Mapper.Map<RoleDTO>(role);
		}

		public async Task UpdateAvatar(string userName, string url)
		{
			var user = await _userManager.FindByNameAsync(userName);
			user.Avatar = url;
			_dbContext.SaveChanges();
		}

		public int GetTotalUser(string groupId)
		{
			return _dbContext.UserGroupRoles.Count(x => x.GroupId == groupId);
		}

		public async Task<UserDTO> GetUserByEmailAsync(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			return CustomMapper.Mapper.Map<UserDTO>(user);
		}
	}
}
