using MediatR;
using Microsoft.AspNetCore.Identity;
using UTCClassSupport.API.Authorize.Responses;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Infrustructure.Repositories;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.JoinGroup
{
  public class JoinGroupHandler : IRequestHandler<JoinGroupCommand, Response>
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly AccessManager _accessManager;
    public JoinGroupHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager,
        AccessManager accessManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
      _accessManager = accessManager;
    }
    public async Task<Response> Handle(JoinGroupCommand request, CancellationToken cancellationToken)
    {
      var userId = request.ClaimsIdentity.FindFirst(ClaimData.UserID).Value;
      var role = await _roleManager.FindByNameAsync(GroupRole.User.ToString());
      _dbContext.UserGroupRoles.Add(new UserGroupRole()
      {
        UserId = userId,
        GroupId = request.GroupId,
        RoleId = role.Id
      });
      _dbContext.SaveChanges();
      return await _accessManager.GetLoginWithGroupToken(request.ClaimsIdentity, request.GroupId);
    }
  }
}
