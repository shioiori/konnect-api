using Konnect.API.Data;
using MediatR;
using UTCClassSupport.API.Responses;

namespace Konnect.API.Application.KickFromGroup
{
	public class KickFromGroupCommand : UserInfo, IRequest<Response>
	{
		public string UserKicked { get; set; }
	}
}
