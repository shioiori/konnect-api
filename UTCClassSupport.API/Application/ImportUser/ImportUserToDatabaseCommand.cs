using MediatR;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.ImportExcel
{
    public class ImportUserToDatabaseCommand : IRequest<ImportUserToDatabaseResponse>
  {
    public string GroupId { get; set; }
    public IFormFile File { get; set; }
  }
}
