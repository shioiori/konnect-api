using Konnect.API.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UTCClassSupport.API.Application.ClearTimetable;
using UTCClassSupport.API.Application.DeleteTimetable;
using UTCClassSupport.API.Application.GetUserTimetable;
using UTCClassSupport.API.Application.ImportExcel;
using UTCClassSupport.API.Application.ImportTimetable;
using UTCClassSupport.API.Application.ScheduleTimetableRemind;
using UTCClassSupport.API.Application.UpdateTimetable;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Controllers
{
    [ApiController]
  [Authorize(AuthenticationSchemes = "Bearer")]
  [Route("timetable")]
  public class TimetableController : BaseController
  {
    private readonly IMediator _mediator;
    public TimetableController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpGet]
    public async Task<Response> GetUserTimetable()
    {
      var data = ReadJWTToken();
      var query = CustomMapper.Mapper.Map<GetUserTimetableQuery>(data);
      return await _mediator.Send(query);
    }

    [HttpDelete]
    public async Task<Response> DeleteUserTimetable()
    {
      var data = ReadJWTToken();
      var command = new DeleteTimetableCommand();
      CustomMapper.Mapper.Map<UserInfo, DeleteTimetableCommand>(data, command);
      return await _mediator.Send(command);
    }

    [HttpPost("synchronize")]
    public async Task<Response> SynchronizeTimetableWithGoogleCalendar(string? id)
    {
      var data = ReadJWTToken();
      var command = new SynchronizeTimetableWithGoogleCalendarCommand();
      CustomMapper.Mapper.Map<UserInfo, SynchronizeTimetableWithGoogleCalendarCommand>(data, command);
      command.TimetableId = id;
      return await _mediator.Send(command);
    }

    [HttpPost("remind/{time}")]
    public async Task<Response> UpdateRemindTimetable(int time)
    {
      var data = ReadJWTToken();
      var command = new UpdateRemindTimetableCommand();
      CustomMapper.Mapper.Map<UserInfo, UpdateRemindTimetableCommand>(data, command);
      command.RemindTime = time;
      return await _mediator.Send(command);
    }

    [HttpPost("event")]
    public async Task<Response> AddEvent(EventRequest eventRequest)
    {
      var data = ReadJWTToken();
      var command = new AddEventCommand();
      CustomMapper.Mapper.Map<UserInfo, AddEventCommand>(data, command);
      CustomMapper.Mapper.Map<EventRequest, AddEventCommand>(eventRequest, command);
      return await _mediator.Send(command);
    }
  }
}
