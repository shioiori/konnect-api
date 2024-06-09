using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Responses.DTOs;

namespace UTCClassSupport.API.Application.GetNews
{
  public class GetPostsHandler : IRequestHandler<GetPostsQuery, GetPostsResponse>
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    public GetPostsHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
    }
    public async Task<GetPostsResponse> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
            try
            {
                var query = _dbContext.Bulletins;
                if (request.State == null)
                {
                    return new GetPostsResponse()
                    {
                        Success = true,
                        Posts = CustomMapper.Mapper.Map<IEnumerable<PostDTO>>(query.ToArray())
                    };
                }
                var res = await query.Where(x => x.GroupId == request.GroupId && x.Approved == (int)request.State)
                                  .Include(x => x.Comments)
                                  .OrderByDescending(x => x.CreatedDate)
                                  .ToArrayAsync();
                var posts = CustomMapper.Mapper.Map<IEnumerable<PostDTO>>(res);
                foreach (var post in posts)
                {
                    post.User = CustomMapper.Mapper.Map<UserDTO>(await _userManager.FindByNameAsync(post.CreatedBy));
                    foreach (var comment in post.Comments)
                    {
                        comment.User = CustomMapper.Mapper.Map<UserDTO>(await _userManager.FindByNameAsync(comment.CreatedBy));
                    }
                }
                return new GetPostsResponse()
                {
                    Success = true,
                    Posts = posts
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
    }
  }
}
