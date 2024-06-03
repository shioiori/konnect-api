using Konnect.API.Infrustructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Repositories;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Responses.DTOs;

namespace Konnect.API.Application.GetPost
{
  public class GetPostHandler : IRequestHandler<GetPostQuery, GetPostResponse>
  {
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    public GetPostHandler(IPostRepository postRepository, IUserRepository userRepository)
    {
      _postRepository = postRepository;
      _userRepository = userRepository;
    }
    public async Task<GetPostResponse> Handle(GetPostQuery request, CancellationToken cancellationToken)
    {
      var post = _postRepository.GetPost(request.PostId);
      var dto = CustomMapper.Mapper.Map<PostDTO>(post);
      var user = await _userRepository.GetUserAsync(dto.CreatedBy);
      dto.User = user;
      return new GetPostResponse()
      {
        Success = true,
        Type = ResponseType.Success,
        Post = dto
      };
    }
  }
}
