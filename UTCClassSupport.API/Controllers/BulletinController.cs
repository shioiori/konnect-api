using MediatR;
using Microsoft.AspNetCore.Mvc;
using UTCClassSupport.API.Application.ChangeNewsState;
using UTCClassSupport.API.Application.UploadNewsToBulletin;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Controllers
{
  public class BulletinController
  {
    private readonly IMediator _mediator;
    public BulletinController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost]
    public async Task<UploadNewsToBulletinResponse> UploadNewsToBulletin(BulletinDTO bulletinDTO)
    {
      var bulletin = CustomMapper.Mapper.Map<UploadNewsToBulletinCommand>(bulletinDTO);
      return await _mediator.Send(bulletin);
    }

    [HttpPost]
    public async Task<ChangeNewsStateResponse> ChangeNewState(string postId, ApproveProcess process)
    {
      return await _mediator.Send(new ChangeNewsStateCommand()
      {
        PostId = postId,
        State = process
      });
    }
  }
}
