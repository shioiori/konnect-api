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

    [HttpPost("{postId}/state/{process}")]
    public async Task<ChangePostStateResponse> ChangeNewState(string postId, ApproveProcess process)
    {
      var data = ReadJWTToken();
      var bulletin = new ChangePostStateCommand()
      {
        PostId = postId,
        State = process
      };
      CustomMapper.Mapper.Map<UserInfo, ChangePostStateCommand>(data, bulletin);
      return await _mediator.Send(bulletin);
    }

    [HttpGet("{state?}")]
    public async Task<GetPostResponse> GetNewsAsync(ApproveProcess? state)
    {
      var bulletin = new GetPostQuery()
      {
        State = state
      };
      return await _mediator.Send(bulletin);
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
