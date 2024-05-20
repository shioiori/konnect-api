using Konnect.ChatHub.Models;
using Konnect.ChatHub.Models.Database;
using Microsoft.Extensions.Options;

namespace Konnect.ChatHub.Repositories
{

  public class UserRepository : BaseRepository<User>, IUserRepository
  {
    public UserRepository(
        IOptions<ChatDatabaseSettings> ChatDatabaseSettings) : base(ChatDatabaseSettings)
    {
    }
  }

  public interface IUserRepository : IBaseRepository<User>
  {
  }
}
