using Microsoft.AspNetCore.Mvc;
using UTCClassSupport.Chat.Repositories;

namespace UTCClassSupport.Chat.Controllers
{
  public class ChatController : ControllerBase
  {
    public void CreateChat(string groupId, string[] users) { }

    public void GetChat(string id)
    {

    }

    public void GetChats(string groupId, string userId, string? name) { }
  }
}
