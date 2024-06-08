using Konnect.API.Data;
using MediatR;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace Konnect.API.Application.EditGroup
{
	public class EditGroupCommand : UserInfo, IRequest<Response>
	{
		public string EditGroupId { get; set; }
		public GroupDTO Group { get; set; }
	}
}
