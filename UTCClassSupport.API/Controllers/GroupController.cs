using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UTCClassSupport.API.Application.GetGroupByUser;
using UTCClassSupport.API.Application.JoinGroup;
using UTCClassSupport.API.Application.OutGroup;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Controllers
{
  [Route("group")]
  [Authorize(AuthenticationSchemes = "Bearer")]
  public class GroupController : BaseController
  {
    private readonly IMediator _mediator;
    public GroupController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpGet]
    public async Task<Response> GetGroupByUserAsync()
    {
      var identity = HttpContext.User.Identity as ClaimsIdentity;
      var userId = identity.FindFirst(ClaimData.UserID).Value;
      return await _mediator.Send(new GetGroupByUserQuery()
      {
        UserId = userId
      });
    }

    [HttpPost("join/{id}")]
    public async Task<Response> JoinGroup(string id)
    {
      var identity = HttpContext.User.Identity as ClaimsIdentity;
      var userId = identity.FindFirst(ClaimData.UserID).Value;
      return await _mediator.Send(new JoinGroupCommand()
      {
        UserId = userId,
        GroupId = id,
      });
    }

    [HttpDelete("{id?}/out")]
    public async Task<Response> OutGroup(string? id)
    {
      var data = ReadJWTToken();
      var command = new OutGroupCommand();
      CustomMapper.Mapper.Map<UserData, OutGroupCommand>(data, command);
      command.CurrentGroupId = id;
      return await _mediator.Send(command);
    }
  }
}
