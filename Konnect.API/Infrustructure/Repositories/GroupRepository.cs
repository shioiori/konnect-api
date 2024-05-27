using Konnect.API.Common;
using Microsoft.AspNetCore.Identity;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests;

namespace Konnect.API.Infrustructure.Repositories
{
  public interface IGroupRepository
  {
    void AddGroup(AddGroupRequest request, string userId);
  }

  public class GroupRepository : IGroupRepository
  {
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly EFContext _dbContext;

    public GroupRepository(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IConfiguration configuration,
        EFContext dbContext)
    {
      _userManager = userManager;
      _roleManager = roleManager;
      _configuration = configuration;
      _dbContext = dbContext;
    }

    public void AddGroup(AddGroupRequest request, string userId)
    {
      var group = CustomMapper.Mapper.Map<Group>(request);
      _dbContext.Groups.Add(group);
      _dbContext.SaveChanges();
      var link = new UserGroupRole()
      {
        GroupId = group.Id,
        UserId = userId,
        RoleId = AppData.ManagerRole,
      };
      _dbContext.UserGroupRoles.Add(link);
      _dbContext.SaveChanges();
    }
  }
}
