using Konnect.ChatHub.Models;
using Konnect.ChatHub.Models.Database;
using Microsoft.Extensions.Options;

namespace Konnect.ChatHub.Repositories
{

  public class MessageRepository : BaseRepository<Message>, IMessageRepository
  {
    public MessageRepository(
        IOptions<ChatDatabaseSettings> ChatDatabaseSettings) : base(ChatDatabaseSettings)
    {
    }
  }

  public interface IMessageRepository : IBaseRepository<Message>
  {
  }
}
