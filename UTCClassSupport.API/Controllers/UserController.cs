using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UTCClassSupport.API.Requests;

namespace UTCClassSupport.API.Controllers
{
  [Authorize(AuthenticationSchemes = "Bearer")]
  [Route("user")]
  public class UserController : BaseController
  {
    [HttpGet]
    public UserData GetUserData()
    {
      return ReadJWTToken();
    }
  }
}
