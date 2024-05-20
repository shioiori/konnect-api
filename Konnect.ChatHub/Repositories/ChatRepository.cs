using Konnect.ChatHub.Models;
using Konnect.ChatHub.Models.Database;
using Microsoft.Extensions.Options;

namespace Konnect.ChatHub.Repositories
{
  public class ChatRepository : BaseRepository<Chat>, IChatRepository
  {
    public ChatRepository(
        IOptions<ChatDatabaseSettings> ChatDatabaseSettings) : base(ChatDatabaseSettings)
    {
    }
  }

  public interface IChatRepository
  {
  }

  public class UserRepository : BaseRepository<User>, IUserRepository
  {
    public UserRepository(
        IOptions<ChatDatabaseSettings> ChatDatabaseSettings) : base(ChatDatabaseSettings)
    {
    }
  }

  public interface IUserRepository
  {
  }

  public class MessageRepository : BaseRepository<Message>, IMessageRepository
  {
    public MessageRepository(
        IOptions<ChatDatabaseSettings> ChatDatabaseSettings) : base(ChatDatabaseSettings)
    {
    }
  }

  public interface IMessageRepository
  {
  }
}
