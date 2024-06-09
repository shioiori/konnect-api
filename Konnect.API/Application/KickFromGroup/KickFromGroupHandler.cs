using Konnect.API.Infrustructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Common.Mail;
using UTCClassSupport.API.Infrustructure.Repositories;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Utilities;

namespace Konnect.API.Application.KickFromGroup
{
	public class KickFromGroupHandler : IRequestHandler<KickFromGroupCommand, Response>
	{
		private readonly IUserRepository _userRepository;
		private readonly IGroupRepository _groupRepository;
		private readonly NotificationManager _notificationManager;
		public KickFromGroupHandler(IUserRepository userRepository,
			IGroupRepository groupRepository,
			NotificationManager notificationManager,
			IMailHandler mailHandler)
		{
			_userRepository = userRepository;
			_groupRepository = groupRepository;
			_notificationManager = notificationManager;
		}
		public async Task<Response> Handle(KickFromGroupCommand request, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetUserAsync(request.UserKicked);
			var success = await _userRepository.RemoveRoleFromGroupAsync(request.UserKicked, request.GroupId);
			if (success)
			{
				var group = _groupRepository.GetGroup(request.GroupId);
				_notificationManager.NotifyKickFromGroup(group, user, request);
			}
			return new Response()
			{
				Success = true,
				Message = "Người dùng đã rời khỏi nhóm",
				Type = ResponseType.Success,
			};
		}
	}
}
