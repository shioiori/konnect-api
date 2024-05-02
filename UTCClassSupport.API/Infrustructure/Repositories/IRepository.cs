namespace UTCClassSupport.API.Infrustructure.Repositories
{
  public interface ICommandRepository<TRequest, TResponse, TEntity, key>
  {
    TResponse Add(TRequest request);
    void Update(key id, TRequest request);
    void Delete(key id);
  }

  public interface IQueryRepository<TRequest, TResponse, TEntity, key>
  {
    IEnumerable<TResponse> GetAll();
    IEnumerable<TResponse> Get(key id);
  }
}
