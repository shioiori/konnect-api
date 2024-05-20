using Microsoft.AspNetCore.Mvc;
using UTCClassSupport.Chat.Repositories;

namespace Konnect.ChatHub.Controllers
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
