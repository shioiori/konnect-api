using Konnect.ChatHub.Models.Database;
using Microsoft.Extensions.Options;

namespace Konnect.ChatHub.Repositories
{
  public interface IUnitOfWork 
  {
    IChatRepository Chats { get; }
    IMessageRepository Messages { get; }
    IUserRepository Users { get; }
    IGroupRepository Groups { get; }
  }
  public class UnitOfWork : IUnitOfWork
  {
    private readonly IOptions<ChatDatabaseSettings> _chatDatabaseSetting;
    public UnitOfWork(IOptions<ChatDatabaseSettings> chatDatabaseSettings,
      IChatRepository chatRepository,
      IUserRepository userRepository,
      IMessageRepository messageRepository,
      IGroupRepository groupRepository)
    {
      _chatDatabaseSetting = chatDatabaseSettings;
      Chats = chatRepository;
      Users = userRepository;
      Messages = messageRepository;
      Groups = groupRepository;
    }

    public IChatRepository Chats { get; }

    public IMessageRepository Messages { get; }

    public IUserRepository Users { get; }

    public IGroupRepository Groups { get; }
  }
}
