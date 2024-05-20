using MediatR;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.ImportTimetable
{
  public class ImportTimetableCommand : UserData, IRequest<ImportResponse>
  {
    public IFormFile File { get; set; }
  }
}
