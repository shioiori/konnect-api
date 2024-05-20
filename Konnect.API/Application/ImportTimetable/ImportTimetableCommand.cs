using Konnect.API.Data;
using MediatR;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.ImportTimetable
{
    public class ImportTimetableCommand : UserInfo, IRequest<ImportResponse>
  {
    public IFormFile File { get; set; }
  }
}
