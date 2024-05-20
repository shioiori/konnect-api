using UTCClassSupport.API.Responses;
using MediatR;
using UTCClassSupport.API.Common;
using Konnect.API.Data;

namespace UTCClassSupport.API.Application.GetNews
{
    public class GetPostQuery : UserInfo, IRequest<GetPostResponse>
  {
    public ApproveProcess? State { get; set; }
  }
}
