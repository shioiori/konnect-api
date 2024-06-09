using Konnect.API.Infrustructure.Repositories;
using Konnect.API.Utilities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Infrustructure.Repositories;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Responses.DTOs;

namespace UTCClassSupport.API.Application.OutGroup
{
	public class OutGroupHandler : IRequestHandler<OutGroupCommand, Response>
	{
		private readonly IGroupRepository _groupRepository;
		private readonly IUserRepository _userRepository;
		private readonly AccessManager _accessManager;
		private readonly NotificationManager _notificationManager;
		public OutGroupHandler(IGroupRepository groupRepository, 
			IUserRepository userRepository,
			AccessManager accessManager,
			NotificationManager notificationManager)
		{
			_groupRepository = groupRepository;
			_userRepository = userRepository;
			_accessManager = accessManager;
			_notificationManager = notificationManager;
		}
		public async Task<Response> Handle(OutGroupCommand request, CancellationToken cancellationToken)
		{
			if (request.CurrentGroupId == null)
			{
				request.CurrentGroupId = request.GroupId;
			}
			_groupRepository.RemoveUserFromGroup(request.UserId, request.CurrentGroupId);
			// if there's no user in group, delete it
			if (_groupRepository.CountNumberUserInGroup(request.CurrentGroupId) == 0)
			{
				_groupRepository.DeleteGroup(request.CurrentGroupId);
			}
			else
			{
				// if there's no manager in group, push random to manager
				if (_groupRepository.CountNumberRoleInGroup(request.CurrentGroupId, GroupRole.Manager) == 0)
				{
					var users = _userRepository.GetUsers(request.CurrentGroupId);
					var user = RandomUtilitiy<UserDTO>.Random(users);
					_userRepository.UpdateRoleAsync(user.UserName, GroupRole.Manager.ToString(), request.CurrentGroupId);
					var group = _groupRepository.GetGroup(request.CurrentGroupId);
					_notificationManager.NotifyChangeRole(GroupRole.Manager, group, null, request);
				}
			}
			return await _accessManager.GetLoginToken(new LoginRequest()
			{
				Username = request.UserName,
				IsLogin = true
			});
		}
	}
}
