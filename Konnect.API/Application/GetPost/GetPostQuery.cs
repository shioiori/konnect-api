using Konnect.API.Data;
using MediatR;
using UTCClassSupport.API.Responses;

namespace Konnect.API.Application.GetPost
{
  public class GetPostQuery : UserInfo, IRequest<GetPostResponse>
  {
    public string PostId { get; set; }
  }
}
