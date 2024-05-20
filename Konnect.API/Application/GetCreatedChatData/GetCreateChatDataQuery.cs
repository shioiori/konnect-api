using Konnect.API.Data;
using MediatR;

namespace Konnect.API.Application.GetCreatedChatData
{
    public class GetCreateChatDataQuery : UserInfo, IRequest<GetCreateChatData>
    {
      public string[] UserNames { get; set; }
    }
}
