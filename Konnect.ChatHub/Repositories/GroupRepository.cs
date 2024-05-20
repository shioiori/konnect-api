using Konnect.ChatHub.Models;
using Konnect.ChatHub.Models.Database;
using Microsoft.Extensions.Options;

namespace Konnect.ChatHub.Repositories
{

  public class GroupRepository : BaseRepository<Group>, IGroupRepository
  {
    public GroupRepository(
        IOptions<ChatDatabaseSettings> ChatDatabaseSettings) : base(ChatDatabaseSettings)
    {
    }
  }

  public interface IGroupRepository : IBaseRepository<Group>
  {
  }
}
