using Konnect.API.Application.GetCreatedChatData;
using Konnect.API.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using UTCClassSupport.API.Application.UploadNewsToBulletin;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Controllers
{
  [Authorize(AuthenticationSchemes = "Bearer")]
  [Route("/chat")]
  public class ChatController : BaseController
  {
    private readonly IMediator _mediator;
    public ChatController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpGet("user/data")]
    public async Task<GetCreateChatData> GetCreateChatDataAsync(string[] name)
    {
      var data = ReadJWTToken();
      var query = new GetCreateChatDataQuery();
      CustomMapper.Mapper.Map<UserInfo, GetCreateChatDataQuery>(data, query);
      query.UserNames = name;
      return await _mediator.Send(query);
    }

  }
}
