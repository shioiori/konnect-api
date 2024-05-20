using Konnect.API.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UTCClassSupport.API.Application.GetNotification;
using UTCClassSupport.API.Application.UpdateStateNotification;
using UTCClassSupport.API.Application.UploadNewsToBulletin;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests.Pagination;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
  [Route("notification")]
  public class NotificationController : BaseController
  {
    private readonly IMediator _mediator;
    public NotificationController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpGet]
    public async Task<Response> GetNotificationAsync(bool isPagination)
    {
      try
      {
        var data = ReadJWTToken();
        var command = new GetNotificationQuery();
        CustomMapper.Mapper.Map<UserInfo, GetNotificationQuery>(data, command);
        if (isPagination)
        {
          command.PaginationData = new PaginationData();
        }
        return await _mediator.Send(command);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    [HttpPost("seen")]
    public async Task<Response> UpdateStateNotification()
    {
      var data = ReadJWTToken();
      var command = new UpdateStateNotificationCommand();
      CustomMapper.Mapper.Map<UserInfo, UpdateStateNotificationCommand>(data, command);
      return await _mediator.Send(command);
    }
  }
}
