using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Controllers
{
  [Authorize(AuthenticationSchemes = "Bearer")]
  [Route("/chat")]
  public class ChatController
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
      var chats = _dbContext.Chats.Where(x => string.IsNullOrEmpty(name) || x.Name.ToLower().Contains(name.ToLower()))
                            .Include(x => x.Messages).ToList();
      return chats;
    }

    [HttpGet]
    public Chat GetChat(string id)
    {
      var chat = _dbContext.Chats.FirstOrDefault(x => x.Id == id);
      return chat;
    }

    [HttpPost]
    public Chat AddChat(string[] users)
    {
      throw new NotImplementedException();
      //_dbContext.Chats.Add(chat);
      //_dbContext.SaveChanges();
      //return chat;
    }

    [HttpPost("/message")]
    public Message AddMessage(Message message)
    {
      _dbContext.Messages.Add(message);
      _dbContext.SaveChanges();
      return message;
    }
  }
}
