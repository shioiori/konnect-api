using Konnect.API.Infrustructure.Repositories;
using MediatR;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Repositories;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace Konnect.API.Application.DeleteGroup
{
	public class DeleteGroupHandler : IRequestHandler<DeleteGroupCommand, Response>
	{
		private readonly IGroupRepository _groupRepository;
		private readonly AccessManager _accessManager;
		public DeleteGroupHandler(IGroupRepository groupRepository, AccessManager accessManager)
		{
			_groupRepository = groupRepository;
			_accessManager = accessManager;
		}
		public async Task<Response> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
		{
			try
			{
				_groupRepository.DeleteGroup(request.GroupId);
				return await _accessManager.GetLoginToken(new LoginRequest()
				{
					Username = request.UserName,
					IsLogin = true
				});
			}
			catch (Exception ex)
			{
				return new Response()
				{
					Success = false,
					Message = ex.Message,
					Type = ResponseType.Error
				};
			}
		}
	}
}
