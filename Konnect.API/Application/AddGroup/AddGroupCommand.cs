using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace Konnect.API.Application.AddGroup
{
  public class AddGroupCommand : IRequest<Response>
  {
    public AddGroupRequest Group { get; set; }
    public string UserId { get; set; }
  }
}
