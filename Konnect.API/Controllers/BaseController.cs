using Konnect.API.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UTCClassSupport.API.Common;

namespace UTCClassSupport.API.Controllers
{
    public class BaseController : ControllerBase
  {
    [NonAction]
    public UserInfo ReadJWTToken()
    {
      var identity = HttpContext.User.Identity as ClaimsIdentity;
      return new UserInfo
      {
        UserId = identity.FindFirst(ClaimData.UserID).Value,
        UserName = identity.FindFirst(ClaimData.UserName).Value,
        DisplayName = identity.FindFirst(ClaimData.DisplayName).Value,
        GroupId = identity.FindFirst(ClaimData.GroupID).Value,
        Email = identity.FindFirst(ClaimData.Email).Value,
        Tel = identity.FindFirst(ClaimData.Tel).Value,
        RoleId = identity.FindFirst(ClaimData.RoleID).Value,
        RoleName = identity.FindFirst(ClaimData.RoleName).Value,
      };
    }
  }
}
