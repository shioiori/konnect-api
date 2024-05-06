using MediatR;
using Microsoft.AspNetCore.Identity;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.OutGroup
{
  public class OutGroupHandler : IRequestHandler<OutGroupCommand, OutGroupResponse>
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public OutGroupHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
    }
    public Task<OutGroupResponse> Handle(OutGroupCommand request, CancellationToken cancellationToken)
    {
      if (request.CurrentGroupId == null)
      {
        request.CurrentGroupId = request.GroupId;
      }
      var link = _dbContext.UserGroupRoles.First(x => x.GroupId == request.GroupId && x.UserId == request.UserId);
      _dbContext.UserGroupRoles.Remove(link);
      _dbContext.SaveChanges();
      return Task.FromResult(new OutGroupResponse()
      {
        Success = true,
        Type = ResponseType.Success,
        Message = "Bạn đã rời khỏi group"
      });
    }
  }
}
