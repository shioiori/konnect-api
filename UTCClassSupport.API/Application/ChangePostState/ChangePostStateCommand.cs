using MediatR;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.ChangeNewsState
{
  public class ChangePostStateCommand : IRequest<ChangePostStateResponse>
  {
    public string PostId { get; set; }
    public ApproveProcess State { get; set; }
  }
}
