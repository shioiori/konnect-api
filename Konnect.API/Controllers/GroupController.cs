using DocumentFormat.OpenXml.Office2010.Excel;
using Konnect.API.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UTCClassSupport.API.Application.GetGroupByUser;
using UTCClassSupport.API.Application.InviteToGroup;
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
      return await _mediator.Send(new JoinGroupCommand()
      {
        GroupId = id,
        ClaimsIdentity = identity
      });
    }

    [HttpDelete("out/{id?}")]
    public async Task<Response> OutGroup(string? id)
    {
      var data = ReadJWTToken();
      var command = new OutGroupCommand();
      CustomMapper.Mapper.Map<UserInfo, OutGroupCommand>(data, command);
      command.CurrentGroupId = id;
      return await _mediator.Send(command);
    }

    [HttpPost("invite")]
    public async Task<Response> InvitePeople(InviteToGroupRequest request)
    {
      var data = ReadJWTToken();
      var command = new InviteToGroupCommand();
      CustomMapper.Mapper.Map<UserInfo, InviteToGroupCommand>(data, command);
      CustomMapper.Mapper.Map<InviteToGroupRequest, InviteToGroupCommand>(request, command);
      return await _mediator.Send(command);
    }
  }
}
