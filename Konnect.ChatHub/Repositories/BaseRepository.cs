﻿using Konnect.ChatHub.Models;
using Konnect.ChatHub.Models.Database;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Konnect.ChatHub.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
  {

    protected readonly IMongoCollection<T> _TCollection;
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
    public async Task<T> CreateAsync(T newT)
    {
      await _TCollection.InsertOneAsync(newT);
      return newT;
    }
    public async Task UpdateAsync(string id, T updatedT) =>
        await _TCollection.ReplaceOneAsync(x => x.Id == id, updatedT);
    public async Task RemoveAsync(string id) =>
        await _TCollection.DeleteOneAsync(x => x.Id == id);
  }

  public interface IBaseRepository<T>
  {
    Task<T> GetAsync(string id);
    Task<List<T>> GetAllAsync();
    Task<T> CreateAsync(T obj);
    Task UpdateAsync(string id, T obj);
    Task RemoveAsync(string id);
  }
}

