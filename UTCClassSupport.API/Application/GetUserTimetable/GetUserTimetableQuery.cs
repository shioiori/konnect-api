using MediatR;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.GetUserTimetable
{
    public class GetUserTimetableQuery : UserData, IRequest<GetUserTimetableResponse>
    {
    }
}
