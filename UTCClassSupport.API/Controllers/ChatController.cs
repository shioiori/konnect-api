using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Controllers
{
  [Authorize(AuthenticationSchemes = "Bearer")]
  [Route("/chat")]
  public class ChatController : BaseController
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    public ChatController(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
    }

    [HttpGet("search/{name?}")]
    public List<Chat> GetChats(string? name)
    {
      var userData = ReadJWTToken();
      var chats = _dbContext.Chats
        .Where(x => (string.IsNullOrEmpty(name) 
            || x.Name.ToLower().Contains(name.ToLower()))
            && x.Joinners.Select(x => x.UserName).Contains(userData.UserName))
        .Include(x => x.Messages).ToList();
      return chats;
    }

    [HttpGet("{id}")]
    public Chat GetChat(string id)
    {
      var chat = _dbContext.Chats.FirstOrDefault(x => x.Id == id);
      return chat;
    }

    [HttpGet]
    public async Task<Chat> GetChatAsync(string[] users)
    {
      var userData = ReadJWTToken();
      var chats = _dbContext.Chats.Include(x => x.Joinners).ToList();
      foreach (var room in chats)
      {
        if (room != default && room.Joinners.Count() == users.Length)
        {
          bool isExist = true;
          if (!room.Joinners.Select(x => x.UserName).Contains(userData.UserName)) continue;
          foreach (var join in room.Joinners)
          {
              if (!users.Contains(join.UserName))
              {
                isExist = false;
                break;
              }
          }
          if (isExist)
          {
            return _dbContext.Chats.Where(x => x.Id == room.Id)
              .Include(x => x.Messages).First();
          }
        }
      }
      var data = ReadJWTToken();
      string name = "";
      foreach (var user in users)
      {
        name += user + ", ";
      }
      name = name.Remove(name.Length - 2);
      var chat = new Chat()
      {
        Name = name,
        Avatar = default,
        CreatedDate = DateTime.Now,
        CreatedBy = data.UserName,
        Joinners = new Collection<User>()
      };
      foreach (var username in users)
      {
        var user = await _userManager.FindByNameAsync(username);
        chat.Joinners.Add(user);
      }
      _dbContext.Chats.Add(chat);
      _dbContext.SaveChanges();
      return chat;
    }

    [HttpPost("/message")]
    public Message AddMessage(Message message)
    {
      _dbContext.Messages.Add(message);
      _dbContext.SaveChanges();
      return message;
    }

    [NonAction]
    public ChatResponse MapChatToPluginStructure(Chat chat)
    {
      var chatResponse = new ChatResponse()
      {
        RoomId = chat.Id,
        RoomName = chat.Name,
        Avatar = chat.Avatar,
        Users = chat.Joinners.Select(x => x.UserName).ToList()
      };
      return chatResponse;
    }
  }
}
