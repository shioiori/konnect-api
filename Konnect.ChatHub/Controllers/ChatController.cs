using Konnect.ChatHub.Data;
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

    [HttpGet("exist")]
    public async Task<ChatResponse> IsChatExistAsync(List<UserData> data)
    {
      var users = CustomMapper.Mapper.Map<List<User>>(data);
      var existChat = await _unitOfWork.Chats.GetChat(users);
      if (existChat != default)
      {
        return CustomMapper.Mapper.Map<ChatResponse>(existChat);
      }
      return null;
    }

    [HttpPost]
    public async Task<ChatResponse> CreateChat([FromBody]CreateChatRequest request) {
      try
      {
        string name = "";
        foreach (var user in request.Users)
        {
          name += user.DisplayName + ", ";
        }
        name = name.Substring(0, name.Length - 1);
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
          if (user.Id.ToString() == request.CreatedBy)
          {
            createdBy = user;
          }
          listUser.Add(user);
        }
        name = name.Substring(0, name.Length - 1);
        var chat = new Chat()
        {
          Name = name,
          CreatedDate = DateTime.Now,
          CreatedBy = createdBy,
          Users = listUser,
          Group = group,
        };
        await _unitOfWork.Chats.CreateAsync(chat);
        return CustomMapper.Mapper.Map<ChatResponse>(chat);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    [HttpGet("{id}")]
    public async Task<ChatResponse> GetChat(string id)
    {
      var chat = await _unitOfWork.Chats.GetAsync(id);
      return CustomMapper.Mapper.Map<ChatResponse>(chat);
    }

    [HttpGet]
    public async Task<List<ChatResponse>> GetChatsAsync(string groupId, string userId, string? name) {
      try
      {
        var chats = await _unitOfWork.Chats.GetChats(groupId, userId, name);
        return CustomMapper.Mapper.Map<List<ChatResponse>>(chats);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
