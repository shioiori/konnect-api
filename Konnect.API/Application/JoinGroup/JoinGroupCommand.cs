using MediatR;
using System.Security.Claims;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.JoinGroup
{
  public class JoinGroupCommand : IRequest<Response>
  {
    public string GroupId { get; set; }
    public ClaimsIdentity ClaimsIdentity { get; set; }
  }
}
