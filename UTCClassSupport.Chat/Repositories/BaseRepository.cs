using Microsoft.Extensions.Options;
using UTCClassSupport.Chat.Models;

namespace UTCClassSupport.Chat.Repositories
{
  public class BaseRepository<T> : IBaseRepository<T> where T : class
  {

    private readonly IMongoCollection<T> _TCollection;
    public BaseRepository(
        IOptions<ChatDatabaseSettings> ChatDatabaseSettings)
    {
      var mongoClient = new MongoClient(
          ChatDatabaseSettings.Value.ConnectionString);

      var mongoDatabase = mongoClient.GetDatabase(
          ChatDatabaseSettings.Value.DatabaseName);

      _TCollection = mongoDatabase.GetCollection<T>(CollectionUtils<T>.GetCollectionName());
    }


    public async Task<T> GetAsync(string id) =>
        await _TCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    public async Task<List<T>> GetAllAsync() =>
        await _TCollection.Find(_ => true).ToListAsync();

    public async Task<T> GetAsync(Func<T, bool> expr) => 
      await _TCollection.Find(expr).ToListAsync();

    public async Task CreateAsync(T newT) =>
        await _TCollection.InsertOneAsync(newT);
    public async Task UpdateAsync(string id, T updatedT) =>
        await _TCollection.ReplaceOneAsync(x => x.Id == id, updatedT);
    public async Task RemoveAsync(string id) =>
        await _TCollection.DeleteOneAsync(x => x.Id == id);
  }

  public interface IBaseRepository<T>
  {
    Task<T> GetAsync(string id);
    Task<List<T>> GetAllAsync();
    Task CreateAsync(T obj);
    Task UpdateAsync(string id, T obj);
    Task RemoveAsync(string id);
  }
}

