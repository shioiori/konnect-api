using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UTCClassSupport.API.Controllers
{
  [ApiController]
  [Authorize(AuthenticationSchemes = "Bearer")]
  [Route("schedule")]
  public class ScheduleController : BaseController
  {
    [HttpPost]
    public async Task<> SetSchedule(int minutes)
    {

    }

    [HttpPost]
    public async Task<> StopSchedule()
    {

    }
  }
}
