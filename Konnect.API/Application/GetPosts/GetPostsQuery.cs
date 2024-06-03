using UTCClassSupport.API.Responses;
using MediatR;
using UTCClassSupport.API.Common;
using Konnect.API.Data;

namespace UTCClassSupport.API.Application.GetNews
{
    public class GetPostsQuery : UserInfo, IRequest<GetPostsResponse>
  {
    public ApproveState? State { get; set; }
  }
}
