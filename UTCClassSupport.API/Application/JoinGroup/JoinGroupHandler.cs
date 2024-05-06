using MediatR;
using Microsoft.AspNetCore.Identity;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.JoinGroup
{
  public class JoinGroupHandler : IRequestHandler<JoinGroupCommand, JoinGroupResponse>
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    public JoinGroupHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
    }
    public async Task<JoinGroupResponse> Handle(JoinGroupCommand request, CancellationToken cancellationToken)
    {
      var role = await _roleManager.FindByNameAsync(GroupRole.User.ToString());
      _dbContext.UserGroupRoles.Add(new UserGroupRole()
      {
        UserId = request.UserId,
        GroupId = request.GroupId,
        RoleId = role.Id
      });
      _dbContext.SaveChanges();
      return new JoinGroupResponse()
      {
        Success = true,
        Type = ResponseType.Success,
        Message = "Bạn đã tham gia vào group " + request.GroupId
      };
    }
  }
}
