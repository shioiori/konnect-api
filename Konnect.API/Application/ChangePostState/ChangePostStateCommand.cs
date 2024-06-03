using Konnect.API.Data;
using MediatR;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.ChangeNewsState
{
    public class ChangePostStateCommand : UserInfo, IRequest<ChangePostStateResponse>
  {
    public string PostId { get; set; }
    public string? Message { get; set; }
    public ApproveState State { get; set; }
  }
}
