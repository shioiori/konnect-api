using MediatR;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.ImportTimetable
{
    public class ImportTimetableCommand : IRequest<ImportTimetableResponse>
  {
    public string UserName { get; set; }
    public string GroupId { get; set; }
    public string FilePath { get; set; }
  }
}
