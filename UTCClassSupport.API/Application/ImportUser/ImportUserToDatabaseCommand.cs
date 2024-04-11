using MediatR;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.ImportExcel
{
    public class ImportUserToDatabaseCommand : UserData, IRequest<ImportUserToDatabaseResponse>
  {
    public IFormFile File { get; set; }
  }
}
