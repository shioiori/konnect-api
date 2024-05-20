using Konnect.API.Data;
using MediatR;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Responses.DTOs;

namespace UTCClassSupport.API.Application.ClearTimetable
{
    public class SynchronizeTimetableWithGoogleCalendarCommand : UserInfo, IRequest<SynchronizeTimetableWithGoogleCalendarResponse>
  {
    public string TimetableId { get; set; }
  }
}
