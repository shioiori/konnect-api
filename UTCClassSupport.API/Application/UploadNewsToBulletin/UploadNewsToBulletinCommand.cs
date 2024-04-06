using MediatR;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.UploadNewsToBulletin
{
  public class UploadNewsToBulletinCommand : BaseRequest, IRequest<UploadNewsToBulletinResponse>
  {
    public string Content { get; set; }

  }
}
