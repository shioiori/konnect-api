using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses.DTOs;

namespace Konnect.API.Infrustructure.Repositories
{
  public interface IPostRepository
  {
    Bulletin GetPost(string id);
    void ChangePostState(string id, ApproveState state);
    void AddPost(Bulletin post);
  }

  public class PostRepository : IPostRepository
  {
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly EFContext _dbContext;

    public PostRepository(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IConfiguration configuration,
        EFContext dbContext)
    {
      _userManager = userManager;
      _roleManager = roleManager;
      _configuration = configuration;
      _dbContext = dbContext;
    }

    public Bulletin GetPost(string id)
    {
      var post = _dbContext.Bulletins.Where(x => x.Id == id).Include(x => x.Comments).FirstOrDefault();
      return post;
    }

    public void ChangePostState(string id, ApproveState state)
    {
      var post = GetPost(id);
      post.Approved = (int)state;
      _dbContext.SaveChanges();
    }

    public void AddPost(Bulletin post)
    {
      _dbContext.Bulletins.Add(post);
      _dbContext.SaveChanges();
    }
  }
}
