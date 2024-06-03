using Konnect.API.Application.GetPost;
using Konnect.API.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UTCClassSupport.API.Application.AddPostComment;
using UTCClassSupport.API.Application.ChangeNewsState;
using UTCClassSupport.API.Application.GetNews;
using UTCClassSupport.API.Application.UploadNewsToBulletin;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
  [Route("bulletin")]
  public class BulletinController : BaseController
  {
    private readonly IMediator _mediator;
    public BulletinController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost("")]
    public async Task<UploadNewsToBulletinResponse> UploadNewsToBulletin([FromBody] BulletinRequest bulletin)
    {
      var data = ReadJWTToken();
      var command = new UploadNewsToBulletinCommand();
      CustomMapper.Mapper.Map<UserInfo, UploadNewsToBulletinCommand>(data, command);
      CustomMapper.Mapper.Map<BulletinRequest, UploadNewsToBulletinCommand>(bulletin, command);
      return await _mediator.Send(command);
    }

    [HttpPost("{postId}/state")]
    public async Task<ChangePostStateResponse> ChangeNewState([FromBody] ChangePostStateRequest request)
    {
      var data = ReadJWTToken();
      var command = new ChangePostStateCommand();
      CustomMapper.Mapper.Map<UserInfo, ChangePostStateCommand>(data, command);
      CustomMapper.Mapper.Map<ChangePostStateRequest, ChangePostStateCommand>(request, command);
      return await _mediator.Send(command);
    }

    [HttpGet]
    public async Task<GetPostsResponse> GetPostsAsync(ApproveState? state)
    {
      var data = ReadJWTToken();
      var query = new GetPostsQuery()
      {
        State = state
      };
      CustomMapper.Mapper.Map<UserInfo, GetPostsQuery>(data, query);
      return await _mediator.Send(query);
    }

    [HttpGet("{id}")]
    public async Task<GetPostResponse> GetPostAsync(string id)
    {
      var data = ReadJWTToken();
      var query = new GetPostQuery()
      {
        PostId = id
      };
      CustomMapper.Mapper.Map<UserInfo, GetPostQuery>(data, query);
      return await _mediator.Send(query);
    }

    [HttpPost("{id}/comment")]
    public async Task<AddPostCommentResponse> AddPostCommentAsync(string id, [FromBody] CommentRequest comment)
    {
      var data = ReadJWTToken();
      var command = new AddPostCommentCommand();
      CustomMapper.Mapper.Map<UserInfo, AddPostCommentCommand>(data, command);
      CustomMapper.Mapper.Map<CommentRequest, AddPostCommentCommand>(comment, command);
      command.PostId = id;
      return await _mediator.Send(command);
    }
  }
}
