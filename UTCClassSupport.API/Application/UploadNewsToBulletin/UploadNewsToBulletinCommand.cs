using MediatR;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.UploadNewsToBulletin
{
  public class UploadNewsToBulletinCommand : IRequest<UploadNewsToBulletinResponse>
  {
    public string UserName { get; set; }
    public string Content { get; set; }
    public string GroupId { get; set; }
  }
}
