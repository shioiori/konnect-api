using Konnect.API.Common;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Asn1.Ocsp;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests;

namespace Konnect.API.Infrustructure.Repositories
{
	public interface IGroupRepository
	{
		Group GetGroup(string groupId);
		void AddGroup(AddGroupRequest request, string userId);
		void EditGroup(string groupId, GroupDTO model);
		void DeleteGroup(string groupId);
		void RemoveUserFromGroup(string userId, string groupId);
		int CountNumberUserInGroup(string groupId);
		int CountNumberRoleInGroup(string groupId, GroupRole role);
	}

	public class GroupRepository : IGroupRepository
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;
		private readonly IConfiguration _configuration;
		private readonly EFContext _dbContext;

		public GroupRepository(
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

		public Group GetGroup(string groupId)
		{
			return _dbContext.Groups.FirstOrDefault(x => x.Id == groupId);
		}

		public void AddGroup(AddGroupRequest request, string userId)
		{
			var group = CustomMapper.Mapper.Map<Group>(request);
			_dbContext.Groups.Add(group);
			_dbContext.SaveChanges();
			var link = new UserGroupRole()
			{
				GroupId = group.Id,
				UserId = userId,
				RoleId = AppData.ManagerRole,
			};
			_dbContext.UserGroupRoles.Add(link);
			_dbContext.SaveChanges();
		}

		public void EditGroup(string groupId, GroupDTO model)
		{
			var group = _dbContext.Groups.FirstOrDefault(x => x.Id == groupId);
			model.Id = groupId;
			CustomMapper.Mapper.Map<GroupDTO, Group>(model, group);
			_dbContext.SaveChanges();
		}

		public void DeleteGroup(string groupId)
		{
			var group = _dbContext.Groups.FirstOrDefault(x => x.Id == groupId);
			_dbContext.Groups.Remove(group);
			_dbContext.SaveChanges();
		}

		public void RemoveUserFromGroup(string userId, string groupId)
		{
			var link = _dbContext.UserGroupRoles.First(x => x.GroupId == groupId && x.UserId == userId);
			_dbContext.UserGroupRoles.Remove(link);
			_dbContext.SaveChanges();
		}

		public int CountNumberUserInGroup(string groupId)
		{
			return _dbContext.UserGroupRoles.Count(x => x.GroupId == groupId);
		}

		public int CountNumberRoleInGroup(string groupId, GroupRole role)
		{
			switch (role)
			{
				case GroupRole.Manager:
					return _dbContext.UserGroupRoles.Count(x => x.GroupId == groupId 
							&& x.RoleId == DBRoleSetting.Manager);
				case GroupRole.User:
				default:
					return _dbContext.UserGroupRoles.Count(x => x.GroupId == groupId 
							&& x.RoleId == DBRoleSetting.User);
			}
		}
	}
}
