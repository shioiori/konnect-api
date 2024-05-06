using MediatR;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.JoinGroup
{
  public class JoinGroupCommand : IRequest<JoinGroupResponse>
  {
    public string UserId { get; set; }
    public string GroupId { get; set; }
  }
}
