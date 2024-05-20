using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Requests;
using MediatR;
using UTCClassSupport.API.Common;

namespace UTCClassSupport.API.Application.GetNews
{
  public class GetPostQuery : UserData, IRequest<GetPostResponse>
  {
    public ApproveProcess? State { get; set; }
  }
}
