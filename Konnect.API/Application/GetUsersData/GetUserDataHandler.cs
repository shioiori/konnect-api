using Konnect.API.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;

namespace Konnect.API.Application.GetUsersData
{
  public class GetUserDataHandler : IRequestHandler<GetUsersDataQuery, List<UserData>>
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    public GetUserDataHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
    }
    public async Task<List<UserData>> Handle(GetUsersDataQuery request, CancellationToken cancellationToken)
    {
      var users = new List<UserData>();
      foreach (var username in request.UserNames)
      {
        var user = await _userManager.FindByNameAsync(username);
        users.Add(CustomMapper.Mapper.Map<UserData>(user));
      }
      return users;
    }
  }
}
