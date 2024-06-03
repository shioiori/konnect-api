using Konnect.API.Data;
using MediatR;

namespace Konnect.API.Application.GetUsersData
{
  public class GetUsersDataQuery : IRequest<List<UserData>>
  {
    public string[] UserNames { get; set; }
  }
}
