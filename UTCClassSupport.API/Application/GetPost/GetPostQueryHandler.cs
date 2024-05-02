using MediatR;
using Microsoft.EntityFrameworkCore;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Responses.DTOs;

namespace UTCClassSupport.API.Application.GetNews
{
  public class GetPostQueryHandler : IRequestHandler<GetPostQuery, GetPostResponse>
  {
    private readonly EFContext _dbContext;
    public GetPostQueryHandler(EFContext dbContext)
    {
      _dbContext = dbContext;
    }
    public async Task<GetPostResponse> Handle(GetPostQuery request, CancellationToken cancellationToken)
    {
      var query = _dbContext.Bulletins;
      if (request.State == null)
      {
        return new GetPostResponse()
        {
          Success = true,
          Posts = CustomMapper.Mapper.Map<IEnumerable<PostDTO>>(query.ToArray())
        };
      }
      var res = await query.Where(x => x.Approved == (int)request.State)
                        .Include(x => x.Comments)
                        .OrderByDescending(x => x.CreatedDate)
                        .ToArrayAsync();
      return new GetPostResponse()
      {
        Success = true,
        Posts = CustomMapper.Mapper.Map<IEnumerable<PostDTO>>(res)
      };
    }
  }
}
