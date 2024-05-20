using Konnect.ChatHub.Mapper;
using Konnect.ChatHub.Models;
using Konnect.ChatHub.Repositories;
using Konnect.ChatHub.Requests;
using Konnect.ChatHub.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Konnect.ChatHub.Controllers
{
  [ApiController]
  [Route("/chat")]
  public class ChatController : ControllerBase
  {
    private readonly IUnitOfWork _unitOfWork;
    public ChatController(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    [HttpPost]
    public async Task<ChatResponse> CreateChat([FromBody]CreateChatRequest request) {
      string name = "";
      foreach(var user in request.Users)
      {
        name += user.DisplayName + ", ";
      }
      var group = await _unitOfWork.Groups.GetAsync(request.GroupData.Id);
      if (group == default)
      {
        group = await _unitOfWork.Groups.CreateAsync(CustomMapper.Mapper.Map<Group>(request.GroupData));
      }
      var listUser = new List<User>();
      User createdBy = null;
      foreach (var userData in request.Users)
      {
        var user = await _unitOfWork.Users.GetAsync(userData.Id);
        if (user == default)
        {
          user = await _unitOfWork.Users.CreateAsync(CustomMapper.Mapper.Map<User>(userData));
        }
        if (user.Id == request.CreatedBy)
        {
          createdBy = user;
        }
        listUser.Add(user);
      }
      name = name.Substring(0, name.Length - 1);
      var chat = await _unitOfWork.Chats.CreateAsync(new Chat()
      {
        Name = name,
        CreatedDate = DateTime.Now,
        CreatedBy = createdBy,
        Users = listUser,
        Group = group,
      });
      return CustomMapper.Mapper.Map<ChatResponse>(chat);
    }
    [HttpGet("{id}")]
    public async Task<ChatResponse> GetChat(string id)
    {
      var chat = await _unitOfWork.Chats.GetAsync(id);
      return CustomMapper.Mapper.Map<ChatResponse>(chat);
    }

    [HttpGet]
    public async Task<List<ChatResponse>> GetChatsAsync(string groupId, string userId, string? name) {
      var chats = _unitOfWork.Chats.GetChats(groupId, userId);
      return CustomMapper.Mapper.Map<List<ChatResponse>>(chats);
    }
  }
}
