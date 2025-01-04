using Konnect.API.Infrustructure.Repositories;
using MediatR;
using MimeKit;
using Newtonsoft.Json;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Repositories;
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

		private readonly IMessagePublisher _publisher;
		public InviteToGroupHandler(IUserRepository userRepository,
			IGroupRepository groupRepository,
			NotificationManager notificationManager,
            IMessagePublisher messagePublisher)
		{
			_userRepository = userRepository;
			_groupRepository = groupRepository;
			_notificationManager = notificationManager;
			_publisher = messagePublisher;
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
					var message = new MimeMessage();
                    message.To.Add(MailboxAddress.Parse(request.Guest));
					message.Subject = GetInviteMailSubject(request.DisplayName);
					message.Body = GetInviteMailContent(request.DisplayName, group.Name, token);
                    _publisher.Send(message);
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

		private MimeEntity GetInviteMailContent(string sender, string groupName, string accessCode)
		{
			string body = $"Thân mến,\n{sender} mời bạn tham gia group {groupName}. Nếu bạn đồng ý, nhấn vào đường link dưới đây\n {accessCode}\n để nhận lời mời.";
            BodyBuilder builder = new BodyBuilder();
            builder.HtmlBody = body;
            return builder.ToMessageBody();
        }
	}
}
