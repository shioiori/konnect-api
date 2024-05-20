using MediatR;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.GetGroupByUser
{
  public class GetGroupByUserQuery : IRequest<GetGroupByUserResponse>
  {
    public string UserId { get; set; }
  }
}
