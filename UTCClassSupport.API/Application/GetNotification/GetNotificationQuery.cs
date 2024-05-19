using MediatR;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Requests.Pagination;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.GetNotification
{
  public class GetNotificationQuery : UserData, IRequest<GetNotificationResponse>
  {
    public PaginationData? PaginationData { get; set; }
  }
}
