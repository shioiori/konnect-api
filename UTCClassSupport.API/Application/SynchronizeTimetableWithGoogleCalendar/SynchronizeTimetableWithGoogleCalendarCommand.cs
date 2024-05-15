using MediatR;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Responses.DTOs;

namespace UTCClassSupport.API.Application.ClearTimetable
{
  public class SynchronizeTimetableWithGoogleCalendarCommand : UserData, IRequest<SynchronizeTimetableWithGoogleCalendarResponse>
  {
    public string TimetableId { get; set; }
  }
}
