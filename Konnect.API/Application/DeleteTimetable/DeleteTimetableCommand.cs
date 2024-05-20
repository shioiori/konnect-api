using Konnect.API.Data;
using MediatR;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.DeleteTimetable
{
    public class DeleteTimetableCommand : UserInfo, IRequest<DeleteTimetableResponse>
  {

  }
}
