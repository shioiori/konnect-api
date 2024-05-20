using MediatR;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.DeleteTimetable
{
  public class DeleteTimetableCommand : UserData, IRequest<DeleteTimetableResponse>
  {

  }
}
