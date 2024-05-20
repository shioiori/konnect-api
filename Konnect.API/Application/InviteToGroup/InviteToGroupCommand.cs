using Konnect.API.Data;
using MediatR;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.InviteToGroup
{
    public class InviteToGroupCommand : UserInfo, IRequest<InviteToGroupResponse>
  {
    public string Guest { get; set; }
    public bool IsExistUser { get; set; }
  }
}
