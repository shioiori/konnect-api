using Konnect.API.Data;
using MediatR;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.ImportExcel
{
    public class ImportUserToDatabaseCommand : UserInfo, IRequest<ImportUserToDatabaseResponse>
  {
    public IFormFile File { get; set; }
  }
}
