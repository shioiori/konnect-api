namespace UTCClassSupport.Chat.Repositories
{
  public class ChatRepository : BaseRepository<Chat>, IChatRepository
  {
    public ChatRepository(
        IOptions<ChatDatabaseSettings> ChatDatabaseSettings) : base(ChatDatabaseSettings)
    {
    }

    public async Task<List<Chat>> GetUserChatAsync(string user_id)
    {
      return await _TCollection.Find(x => x.MemberIds.Contains(user_id)).ToListAsync();
    }

    public async Task<Chat> GetChatByInfoAsync(string[] members, bool isLimited)
    {
      return await _TCollection.Find(x => x.IsLimited == isLimited && x.MemberIds == members).FirstOrDefaultAsync();
    }
  }

  public interface IChatRepository : IBaseRepository<Chat>
  {
    Task<List<Chat>> GetUserChatAsync(string user_id);
    Task<Chat> GetChatByInfoAsync(string[] members, bool isLimited);
  }
}
