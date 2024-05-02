using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UTCClassSupport.API.Application.GetGroupByUser;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Controllers
{
  [Route("group")]
  public class GroupController : ControllerBase
  {
    private readonly IMediator _mediator;
    public GroupController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<GetGroupByUserResponse> GetGroupByUserAsync()
    {
      var identity = HttpContext.User.Identity as ClaimsIdentity;
      var userId = identity.FindFirst(ClaimData.UserID).Value;
      return await _mediator.Send(new GetGroupByUserQuery()
      {
        UserId = userId
      });
    }
  }
}
