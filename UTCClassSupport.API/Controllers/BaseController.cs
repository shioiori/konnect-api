using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Requests;

namespace UTCClassSupport.API.Controllers
{
  public class BaseController : ControllerBase
  {
    [NonAction]
    public BaseRequest ReadJWTToken()
    {
      var identity = HttpContext.User.Identity as ClaimsIdentity;
      return new BaseRequest
      {
        UserId = identity.FindFirst(ClaimData.UserID).Value,
        UserName = identity.FindFirst(ClaimData.UserName).Value,
        GroupId = identity.FindFirst(ClaimData.GroupID).Value,
        Email = identity.FindFirst(ClaimData.Email).Value,
        Tel = identity.FindFirst(ClaimData.Tel).Value,
        RoleId = identity.FindFirst(ClaimData.RoleID).Value,
        RoleName = identity.FindFirst(ClaimData.RoleName).Value,
      };
    }
  }
}
