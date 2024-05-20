using Microsoft.AspNetCore.SignalR;
using UTCClassSupport.Chat.Requests;

namespace UTCClassSupport.Chat.Hubs
{
  public class ChatHub
  {
    public class ChatHub : Hub, IDisposable
    {
      private static Dictionary<string, List<string>> userStateConnections;
      private readonly IMessage _messageService;
      private readonly IUser _userService;
      private readonly IChat _chatService;

      public ChatHub( messageService, IUser userService, IChat chatService)
      {
        _messageService = messageService;
        _userService = userService;
        _chatService = chatService;
        if (userStateConnections == null)
        {
          userStateConnections = new Dictionary<string, List<string>>();
        }
      }

      public override Task OnConnectedAsync()
      {
        return base.OnConnectedAsync();
      }

      public override Task OnDisconnectedAsync(Exception? exception)
      {
        return base.OnDisconnectedAsync(exception);
      }

      public async Task OnConnectedNetwork(string user_id)
      {
        if (!userStateConnections.ContainsKey(user_id))
        {
          userStateConnections.Add(user_id, new List<string>());
          userStateConnections[user_id].Add(Context.ConnectionId);
        }
        userStateConnections[user_id].Add(Context.ConnectionId);
        await Clients.Client(Context.ConnectionId).SendAsync("OnConnected", Context.ConnectionId);
      }

      public Task OnDisconnectedNetwork(string userId)
      {
        if (userStateConnections.ContainsKey(userId))
        {
          userStateConnections.Remove(userId);
        }
        return OnDisconnectedAsync(null);
      }

      public async Task SendMessage(string chatId, AddMessageRequest message)
      {

      }

    }
  }
}
