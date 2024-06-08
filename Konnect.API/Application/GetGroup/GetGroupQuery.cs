using Konnect.API.Data;
using MediatR;
using UTCClassSupport.API.Responses;

namespace Konnect.API.Application.GetGroup
{
	public class GetGroupQuery : UserInfo, IRequest<GetGroupResponse>
	{
		public string GroupKey { get; set; }
	}
}
