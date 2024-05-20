using Konnect.API.Data;
using MediatR;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.GetUserTimetable
{
    public class GetUserTimetableQuery : UserInfo, IRequest<GetUserTimetableResponse>
    {
    }
}
