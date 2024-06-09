using DocumentFormat.OpenXml.Office2010.Excel;
using Konnect.API.Application.AddGroup;
using Konnect.API.Application.DeleteGroup;
using Konnect.API.Application.EditGroup;
using Konnect.API.Application.GetGroup;
using Konnect.API.Application.KickFromGroup;
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
		private IRequest<Response> command;

		public GroupController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("all")]
		public async Task<Response> GetGroupByUserAsync()
		{
			var identity = HttpContext.User.Identity as ClaimsIdentity;
			var userId = identity.FindFirst(ClaimData.UserID).Value;
			return await _mediator.Send(new GetGroupByUserQuery()
			{
				UserId = userId
			});
		}

		[HttpGet("{id?}")]
		public async Task<Response> GetGroupAsync(string? groupId)
		{
			try
			{
				var data = ReadJWTToken();
				var query = new GetGroupQuery();
				CustomMapper.Mapper.Map<UserInfo, GetGroupQuery>(data, query);
				query.GroupKey = groupId;
				if (groupId == default)
				{
					query.GroupKey = query.GroupId;
				}
				return await _mediator.Send(query);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpPost]
		public async Task<Response> AddGroup([FromBody] AddGroupRequest request)
		{
			var identity = HttpContext.User.Identity as ClaimsIdentity;
			var userId = identity.FindFirst(ClaimData.UserID).Value;
			return await _mediator.Send(new AddGroupCommand()
			{
				Group = request,
				UserId = userId
			});
		}

		[HttpPost("{id}")]
		public async Task<Response> EditGroup(string id, [FromBody] GroupDTO request)
		{
			var data = ReadJWTToken();
			var command = new EditGroupCommand();
			CustomMapper.Mapper.Map<UserInfo, EditGroupCommand>(data, command);
			command.EditGroupId = id;
			command.Group = request;
			return await _mediator.Send(command);
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
			if (id == default)
			{
				command.CurrentGroupId = command.GroupId;
			}
			return await _mediator.Send(command);
		}

		[HttpPost("invite")]
		public async Task<Response> InvitePeople(InviteToGroupRequest request)
		{
			var data = ReadJWTToken();
			if (request.Guest == data.UserName)
			{
				return new InviteToGroupResponse()
				{
					Success = false,
					Type = ResponseType.Error,
					Message = "Bạn đã ở trong group"
				};
			}
			var command = new InviteToGroupCommand();
			CustomMapper.Mapper.Map<UserInfo, InviteToGroupCommand>(data, command);
			CustomMapper.Mapper.Map<InviteToGroupRequest, InviteToGroupCommand>(request, command);
			return await _mediator.Send(command);
		}

		[HttpDelete]
		public async Task<Response> DeleteGroup(string? groupId)
		{
			var data = ReadJWTToken();
			var command = new DeleteGroupCommand();
			CustomMapper.Mapper.Map<UserInfo, DeleteGroupCommand>(data, command);
			if (groupId == String.Empty)
			{
				command.GroupId = groupId;
			}
			return await _mediator.Send(command);
		}



		[HttpDelete("kick/{username}")]
		public async Task<Response> KickUserFromGroup(string username)
		{
			try
			{
				var data = ReadJWTToken();
				var command = new KickFromGroupCommand();
				CustomMapper.Mapper.Map<UserInfo, KickFromGroupCommand>(data, command);
				command.UserKicked = username;
				return await _mediator.Send(command);
			}
			catch (Exception ex)
			{
				return new Response()
				{
					Success = false,
					Type = ResponseType.Error,
					Message = ex.Message,
				};
			}
		}
	}
}
