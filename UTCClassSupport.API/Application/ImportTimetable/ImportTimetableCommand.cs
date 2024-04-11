using MediatR;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.ImportTimetable
{
  public class ImportTimetableCommand : UserData, IRequest<ImportTimetableResponse>
  {
    public IFormFile File { get; set; }
  }
}
