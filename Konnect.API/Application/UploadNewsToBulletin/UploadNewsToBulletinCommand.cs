using Konnect.API.Data;
using MediatR;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.UploadNewsToBulletin
{
    public class UploadNewsToBulletinCommand : UserInfo, IRequest<UploadNewsToBulletinResponse>
  {
    public string Content { get; set; }

  }
}
