using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UTCClassSupport.API.Application.ChangeNewsState;
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
    public async Task<UploadNewsToBulletinResponse> UploadNewsToBulletin(BulletinDTO bulletinDTO)
    {
      var data = ReadJWTToken();
      var bulletin = new UploadNewsToBulletinCommand();
      CustomMapper.Mapper.Map<BaseRequest, UploadNewsToBulletinCommand>(data, bulletin);
      CustomMapper.Mapper.Map<BulletinDTO, UploadNewsToBulletinCommand>(bulletinDTO, bulletin);
      return await _mediator.Send(bulletin);
    }

    [HttpPost("{postId}/state/{process}")]
    public async Task<ChangeNewsStateResponse> ChangeNewState(string postId, ApproveProcess process)
    {
      var data = ReadJWTToken();
      var bulletin = new ChangeNewsStateCommand();
      CustomMapper.Mapper.Map<BaseRequest, ChangeNewsStateCommand>(data, bulletin);
      bulletin.PostId = postId;
      bulletin.State = process;
      return await _mediator.Send(bulletin);
    }
  }
}
