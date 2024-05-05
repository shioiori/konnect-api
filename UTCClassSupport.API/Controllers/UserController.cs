using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Controllers
{
  [Authorize(AuthenticationSchemes = "Bearer")]
  [Route("user")]
  public class UserController : BaseController
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    public UserController(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
    }

    [HttpGet]
    public UserData GetUserData()
    {
      return ReadJWTToken();
    }

    [HttpGet("group")]
    public List<UserResponse> GetUsers()
    {
      var data = ReadJWTToken();
      var users = _dbContext.UserGroupRoles.Where(x => x.GroupId == data.GroupId)
                            .Include(x => x.User)
                            .Include(x => x.Role)
                            .Select(x => new UserResponse()
                            {
                              Id = x.UserId,
                              Email = x.User.Email,
                              Phone = x.User.PhoneNumber,
                              DisplayName = x.User.Name,
                              UserName = x.User.UserName,
                              Avatar = x.User.Avatar,
                              RoleName = x.Role.Name,
                              GroupId = x.GroupId,
                            }).ToList();
      return users;
      
    }
  }
}
