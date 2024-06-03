using Konnect.ChatHub.Data;
using Konnect.ChatHub.Mapper;
using Konnect.ChatHub.Models;
using Konnect.ChatHub.Models.Database;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Konnect.ChatHub.Repositories
{
  public class ChatRepository : BaseRepository<Chat>, IChatRepository
  {
    public ChatRepository(
        IOptions<ChatDatabaseSettings> ChatDatabaseSettings) : base(ChatDatabaseSettings)
    {
    }

    public Task<List<Chat>> GetChats(string groupId, string userId, string? search)
    {
      var query = _TCollection.Find(x => x.Group.Id == Guid.Parse(groupId) 
        && x.Users.Select(x => x.Id).Contains(Guid.Parse(userId))).ToList();
      var res = query.Where(x => string.IsNullOrEmpty(search) || x.Name.ToLower().Contains(search.ToLower())).ToList();
      return Task.FromResult(res);
    }

    public Chat GetChat(List<User> users)
    {
      var chats = _TCollection.Find(x => x.Users.Count() == users.Count()).ToList();
      var userNames = users.Select(x => x.UserName).ToList();
      foreach (var chat in chats)
      {
        bool flag = true;
        var usersInChat = chat.Users.Select(x => x.UserName);
        foreach (var userName in usersInChat)
        {
          if (userNames.Contains(userName)) continue;
          else
          {
            flag = false;
            break;
          }
        }
        if (flag) return chat;
      }
      return null;
    }

    public async Task AddUsersToChatAsync(string chatId, List<User> users)
    {
      var chat = await GetAsync(chatId);
      var usersInChat = chat.Users.Select(x => x.UserName).ToList();
      foreach (var user in users)
      {
        if (usersInChat.Contains(user.UserName)) continue;
        chat.Users.Add(user);
      }
      await UpdateAsync(chatId, chat);
    }
  }

  public interface IChatRepository : IBaseRepository<Chat>
  {
    Task<List<Chat>> GetChats(string groupId, string userId, string? search);
    Chat GetChat(List<User> users);
    Task AddUsersToChatAsync(string chatId, List<User> users);
  }

}
