using Konnect.API.Infrustructure.Repositories;
using MediatR;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Responses;

namespace Konnect.API.Application.EditGroup
{
	public class EditGroupHandler : IRequestHandler<EditGroupCommand, Response>
	{
		private readonly IGroupRepository _groupRepository;

		public EditGroupHandler(IGroupRepository groupRepository)
		{
			_groupRepository = groupRepository;
		}

		public Task<Response> Handle(EditGroupCommand request, CancellationToken cancellationToken)
		{
			_groupRepository.EditGroup(request.EditGroupId, request.Group);
			return Task.FromResult(new Response()
			{
				Success = true,
				Type = ResponseType.Success,
				Message = "Cập nhật thành công",
			});
		}
	}
}
