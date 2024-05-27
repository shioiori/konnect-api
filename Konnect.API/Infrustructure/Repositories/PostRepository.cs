using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses.DTOs;

namespace Konnect.API.Infrustructure.Repositories
{
  public interface IPostRepository
  {
    Task<PostDTO> GetPost(string id);
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

    public async Task<PostDTO> GetPost(string id)
    {
      var post = _dbContext.Bulletins.Where(x => x.Id == id).Include(x => x.Comments).FirstOrDefault();
      var dto = CustomMapper.Mapper.Map<PostDTO>(post);
      var user = await _userManager.FindByNameAsync(dto.CreatedBy);
      dto.User = CustomMapper.Mapper.Map<UserDTO>(user);
      return dto;
    }
  }
}
