using Konnect.API.Data;
using Konnect.API.Infrustructure.Repositories;
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
using UTCClassSupport.API.Responses.DTOs;

namespace UTCClassSupport.API.Controllers
{
	[ApiController]
	[Authorize(AuthenticationSchemes = "Bearer")]
	[Route("timetable")]
	public class TimetableController : BaseController
	{
		private readonly IMediator _mediator;
		private readonly ITimetableRepository _timetableRepository;
		public TimetableController(IMediator mediator, ITimetableRepository timetableRepository)
		{
			_mediator = mediator;
			_timetableRepository = timetableRepository;
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
		public async Task<Response> AddEvent(AddEventRequest eventRequest)
		{
			var data = ReadJWTToken();
			var command = new AddEventCommand();
			CustomMapper.Mapper.Map<UserInfo, AddEventCommand>(data, command);
			CustomMapper.Mapper.Map<AddEventRequest, AddEventCommand>(eventRequest, command);
			return await _mediator.Send(command);
		}

		[HttpPost("event/{id}")]
		public Response UpdateEvent(int id, [FromBody] UpdateEventRequest request)
		{
			if (_timetableRepository.UpdateEvent(id, request))
			{
				return new Response()
				{
					Success = true,
					Type = ResponseType.Success,
					Message = "Cập nhật thành công",
				};
			}
			else return new Response
			{
				Success = false,
				Type = ResponseType.Error,
				Message = "Có lỗi trong quá trình cập nhật",
			};
		}

		[HttpDelete("event/{id}")]
		public Response DeleteEvent(int id)
		{
			if (_timetableRepository.DeleteEvent(id))
			{
				return new Response()
				{
					Success = true,
					Type = ResponseType.Success,
					Message = "Cập nhật thành công",
				};
			}
			else return new Response
			{
				Success = false,
				Type = ResponseType.Error,
				Message = "Có lỗi trong quá trình xoá",
			};
		}
	}
}
