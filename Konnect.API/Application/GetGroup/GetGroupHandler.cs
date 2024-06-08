using Konnect.API.Infrustructure.Repositories;
using MediatR;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace Konnect.API.Application.GetGroup
{
	public class GetGroupHandler : IRequestHandler<GetGroupQuery, GetGroupResponse>
	{
		private readonly IGroupRepository _groupRepository;
		public GetGroupHandler(IGroupRepository groupRepository) 
		{ 
			_groupRepository = groupRepository;
		}
		public Task<GetGroupResponse> Handle(GetGroupQuery request, CancellationToken cancellationToken)
		{
			try
			{
				var group = _groupRepository.GetGroup(request.GroupKey);
				return Task.FromResult(new GetGroupResponse()
				{
					Success = true,
					Type = ResponseType.Success,
					Group = CustomMapper.Mapper.Map<GroupDTO>(group),
				});
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
