using Konnect.ChatHub.Models.Database;
using Microsoft.Extensions.Options;

namespace Konnect.ChatHub.Repositories
{
  public interface IUnitOfWork 
  {
    IChatRepository Chats { get; }
    IMessageRepository Messages { get; }
    IUserRepository Users { get; }
  }
  public class UnitOfWork : IUnitOfWork
  {
    private readonly IOptions<ChatDatabaseSettings> _chatDatabaseSetting;
    public UnitOfWork(IOptions<ChatDatabaseSettings> chatDatabaseSettings)
    {
      _chatDatabaseSetting = chatDatabaseSettings;
    }
    public IChatRepository Chats
    {
      get
      {
        if (Chats == null)
        {
          return new ChatRepository(_chatDatabaseSetting);
        }
        return Chats;
      }
    }

    public IMessageRepository Messages
    {
      get
      {
        if (Messages == null)
        {
          return new MessageRepository(_chatDatabaseSetting);
        }
        return Messages;
      }
    }

    public IUserRepository Users
    {
      get
      {
        if (Users == null)
        {
          return new UserRepository(_chatDatabaseSetting);
        }
        return Users;
      }
    }
  }
}
