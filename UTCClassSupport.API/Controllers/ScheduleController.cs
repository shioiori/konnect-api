using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Controllers
{
  [ApiController]
  [Authorize(AuthenticationSchemes = "Bearer")]
  [Route("schedule")]
  public class ScheduleController : BaseController
  {
    [HttpPost("start")]
    public async Task<ScheduleResponse> SetSchedule(int minutes)
    {
      throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<ScheduleResponse> StopSchedule()
    {
      throw new NotImplementedException();
    }
  }

  public class ScheduleResponse : Response
  {

  }
}
