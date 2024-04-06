using MediatR;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.ImportTimetable
{
  public class ImportTimetableCommand : BaseRequest, IRequest<ImportTimetableResponse>
  {
    public IFormFile File { get; set; }
  }
}
