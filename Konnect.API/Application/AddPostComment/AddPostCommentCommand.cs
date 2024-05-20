using MediatR;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.AddPostComment
{
  public class AddPostCommentCommand : UserData, IRequest<AddPostCommentResponse>
  {
    public string PostId { get; set; }
    public string Content { get; set; }
  }
}
