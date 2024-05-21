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

    public Task<Chat> GetChat(List<User> users)
    {
      var query = _TCollection.Find(x => x.Users.Count() == users.Count());
      foreach (var user in users)
      {
        query = _TCollection.Find(x => x.Users.Contains(user));
      }
      return Task.FromResult(query.FirstOrDefault());
    }
  }

  public interface IChatRepository : IBaseRepository<Chat>
  {
    Task<List<Chat>> GetChats(string groupId, string userId, string? search);
    Task<Chat> GetChat(List<User> users);
  }

}
