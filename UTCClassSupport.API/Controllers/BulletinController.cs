using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UTCClassSupport.API.Application.ChangeNewsState;
using UTCClassSupport.API.Application.GetNews;
using UTCClassSupport.API.Application.UploadNewsToBulletin;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Mapper;
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
    public async Task<UploadNewsToBulletinResponse> UploadNewsToBulletin(BulletinRequest bulletinDTO)
    {
      var data = ReadJWTToken();
      var bulletin = new UploadNewsToBulletinCommand();
      CustomMapper.Mapper.Map<UserData, UploadNewsToBulletinCommand>(data, bulletin);
      CustomMapper.Mapper.Map<BulletinRequest, UploadNewsToBulletinCommand>(bulletinDTO, bulletin);
      return await _mediator.Send(bulletin);
    }

    [HttpPost("{postId}/state/{process}")]
    public async Task<ChangeNewsStateResponse> ChangeNewState(string postId, ApproveProcess process)
    {
      var data = ReadJWTToken();
      var bulletin = new ChangeNewsStateCommand();
      CustomMapper.Mapper.Map<UserData, ChangeNewsStateCommand>(data, bulletin);
      bulletin.PostId = postId;
      bulletin.State = process;
      return await _mediator.Send(bulletin);
    }

    [HttpGet("{state?}")]
    public async Task<GetNewsResponse> GetNewsAsync(ApproveProcess? state)
    {
      var bulletin = new GetNewsQuery()
      {
        State = state
      };
      return await _mediator.Send(bulletin);
    }
  }
}
