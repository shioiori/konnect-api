using DocumentFormat.OpenXml.Spreadsheet;
using Konnect.API.Infrustructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Common.Mail;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Infrustructure.Repositories;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Utilities;

namespace UTCClassSupport.API.Application.InviteToGroup
{
	public class InviteToGroupHandler : IRequestHandler<InviteToGroupCommand, InviteToGroupResponse>
	{
		private readonly IUserRepository _userRepository;
		private readonly IGroupRepository _groupRepository;
		private readonly NotificationManager _notificationManager;

		private readonly IMailHandler _mailHandler;
		public InviteToGroupHandler(IUserRepository userRepository,
			IGroupRepository groupRepository,
			NotificationManager notificationManager,
			IMailHandler mailHandler)
		{
			_userRepository = userRepository;
			_groupRepository = groupRepository;
			_notificationManager = notificationManager;
			_mailHandler = mailHandler;
		}
		public async Task<InviteToGroupResponse> Handle(InviteToGroupCommand request, CancellationToken cancellationToken)
		{
			if (request.IsExistUser)
			{
				var guest = await _userRepository.GetUserAsync(request.Guest);
				if (guest != null)
				{
					_userRepository.UpdateRoleAsync(guest.UserName, GroupRole.User.ToString(), request.GroupId);
					// notify
					var group = _groupRepository.GetGroup(request.GroupId);
					_notificationManager.NotifyInviteToGroup(group, guest, request);
				}
				else
				{
					return new InviteToGroupResponse()
					{
						Message = "Người dùng này không tồn tại",
						Success = false,
						Type = ResponseType.Error
					};
				}
			}
			else
			{
				var user = await _userRepository.GetUserByEmailAsync(request.Guest);
				if (user != default)
				{
					return new InviteToGroupResponse()
					{
						Message = "Đã có người sử dụng email này",
						Success = false,
						Type = ResponseType.Error
					};
				}
				else
				{
					JoinRequest joinRequest = new JoinRequest()
					{
						GroupId = request.GroupId,
						Email = request.Guest,
					};

					var group = _groupRepository.GetGroup(request.GroupId);
					var key = AesOperation.GenerateKey();
					var encryptText = AesOperation.EncryptString(key, JsonConvert.SerializeObject(joinRequest));
					var token = AesOperation.GenerateToken(key, encryptText);
					_mailHandler.Send(new MailContent()
					{
						To = request.Guest,
						Subject = GetInviteMailSubject(request.DisplayName),
						Body = GetInviteMailContent(request.DisplayName, group.Name, token)
					});
				}
			}
			return new InviteToGroupResponse()
			{
				Message = "Gửi lời mời thành công",
				Success = true,
				Type = ResponseType.Success
			};
		}

		public string GetInviteMailSubject(string sender)
		{
			return "Bạn có một lời mời từ " + sender;
		}

		private string GetInviteMailContent(string sender, string groupName, string accessCode)
		{
			return $"Thân mến,\n{sender} mời bạn tham gia group {groupName}. Nếu bạn đồng ý, nhấn vào đường link dưới đây\n {accessCode}\n để nhận lời mời.";
		}
	}
}
