using Konnect.API.Infrustructure.Repositories;
using MediatR;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Responses;

namespace Konnect.API.Application.GetPost
{
  public class GetPostHandler : IRequestHandler<GetPostQuery, GetPostResponse>
  {
    private readonly IPostRepository _postRepository;
    public GetPostHandler(IPostRepository postRepository)
    {
      _postRepository = postRepository;
    }
    public async Task<GetPostResponse> Handle(GetPostQuery request, CancellationToken cancellationToken)
    {
      var post = await _postRepository.GetPost(request.PostId);
      return new GetPostResponse()
      {
        Success = true,
        Type = ResponseType.Success,
        Post = post
      };
    }
  }
}
