using MediatR;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.InviteToGroup
{
  public class InviteToGroupCommand : UserData, IRequest<InviteToGroupResponse>
  {
    public string Guest { get; set; }
    public bool IsExistUser { get; set; }
  }
}
