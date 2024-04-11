using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Requests;
using MediatR;
using UTCClassSupport.API.Common;

namespace UTCClassSupport.API.Application.GetNews
{
  public class GetNewsQuery : UserData, IRequest<GetNewsResponse>
  {
    public ApproveProcess? State { get; set; }
  }
}
