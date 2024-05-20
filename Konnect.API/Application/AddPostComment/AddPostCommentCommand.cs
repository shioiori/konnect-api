using Konnect.API.Data;
using MediatR;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.AddPostComment
{
    public class AddPostCommentCommand : UserInfo, IRequest<AddPostCommentResponse>
  {
    public string PostId { get; set; }
    public string Content { get; set; }
  }
}
