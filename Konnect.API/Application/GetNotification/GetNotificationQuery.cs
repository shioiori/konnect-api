using Konnect.API.Data;
using MediatR;
using UTCClassSupport.API.Requests.Pagination;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.GetNotification
{
    public class GetNotificationQuery : UserInfo, IRequest<GetNotificationResponse>
  {
    public PaginationData? PaginationData { get; set; }
  }
}
