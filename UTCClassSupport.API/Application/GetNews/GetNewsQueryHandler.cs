using MediatR;
using Microsoft.EntityFrameworkCore;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.GetNews
{
  public class GetNewsQueryHandler : IRequestHandler<GetNewsQuery, GetNewsResponse>
  {
    private readonly EFContext _dbContext;
    public GetNewsQueryHandler(EFContext dbContext)
    {
      _dbContext = dbContext;
    }
    public async Task<GetNewsResponse> Handle(GetNewsQuery request, CancellationToken cancellationToken)
    {
      var query = _dbContext.Bulletins;
      if (request.State == null)
      {
        return new GetNewsResponse()
        {
          Success = true,
          News = CustomMapper.Mapper.Map<IEnumerable<NewsDTO>>(query.ToArray())
        };
      }
      var res = await query.Where(x => x.Approved == (int)request.State).ToArrayAsync();
      return new GetNewsResponse()
      {
        Success = true,
        News = CustomMapper.Mapper.Map<IEnumerable<NewsDTO>>(res)
      };
    }
  }
}
