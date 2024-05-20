using MediatR;
using Microsoft.AspNetCore.Identity;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Infrustructure.Repositories;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.OutGroup
{
  public class OutGroupHandler : IRequestHandler<OutGroupCommand, Response>
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly AccessManager _accessManager;

    public OutGroupHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager,
        AccessManager accessManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
      _accessManager = accessManager;
    }
    public async Task<Response> Handle(OutGroupCommand request, CancellationToken cancellationToken)
    {
      if (request.CurrentGroupId == null)
      {
        request.CurrentGroupId = request.GroupId;
      }
      var link = _dbContext.UserGroupRoles.First(x => x.GroupId == request.GroupId && x.UserId == request.UserId);
      _dbContext.UserGroupRoles.Remove(link);
      _dbContext.SaveChanges();
      var user = await _userManager.FindByNameAsync(request.UserName);
      return await _accessManager.GetLoginToken(new LoginRequest()
      {
        Username = user.UserName,
      });
    }
  }
}
