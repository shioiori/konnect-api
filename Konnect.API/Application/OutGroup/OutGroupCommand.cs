using Konnect.API.Data;
using MediatR;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.OutGroup
{
    public class OutGroupCommand : UserInfo, IRequest<Response>
  {
    public string CurrentGroupId { get; set; }
  }
}
