using Konnect.API.Data;
using MediatR;
using UTCClassSupport.API.Responses;

namespace Konnect.API.Application.DeleteGroup
{
    public class DeleteGroupCommand : UserInfo, IRequest<Response>
    {
        public string GroupId { get; set; }
    }
}
