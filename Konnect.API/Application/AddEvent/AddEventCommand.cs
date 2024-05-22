using Konnect.API.Data;
using MediatR;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.ScheduleTimetableRemind
{
    public class AddEventCommand : UserInfo, IRequest<AddEventResponse>
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Location { get; set; }
  }
}
