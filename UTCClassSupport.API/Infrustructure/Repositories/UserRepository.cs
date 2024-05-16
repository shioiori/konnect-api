using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses.DTOs;

namespace UTCClassSupport.API.Infrustructure.Repositories
{
  public class UserRepository
  {
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly EFContext _dbContext;

    public UserRepository(
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

    public List<UserDTO> GetUsers(string groupId)
    {
      var user = _dbContext.UserGroupRoles.Where(x => x.GroupId == groupId)
                            .Include(x => x.User)
                            .Include(x => x.Role)
                            .Select(x => new UserDTO()
                            {
                              Id = x.UserId,
                              Email = x.User.Email,
                              PhoneNumber = x.User.PhoneNumber,
                              DisplayName = x.User.DisplayName,
                              UserName = x.User.UserName,
                              Avatar = x.User.Avatar,
                              RoleName = x.Role.Name,
                              GroupId = x.GroupId,
                            }).ToList();
      return user;
    }

    public async Task<UserDTO> GetUserAsync(string userName)
    {
      var user = await _userManager.FindByNameAsync(userName);
      var dto = CustomMapper.Mapper.Map<UserDTO>(user);
      return dto;
    }

    public async Task<UserDTO> AddUserAsync(AddUserRequest request)
    {
      var user = CustomMapper.Mapper.Map<User>(request);
      await _userManager.CreateAsync(user);
      _userManager.AddPasswordAsync(user, request.UserName);
      _dbContext.SaveChanges();
      if (request.UserGroupData.RoleName == default)
      {
        request.UserGroupData.RoleName = GroupRole.User.ToString();
      }
      _dbContext.UserGroupRoles.Add(new UserGroupRole()
      {
        GroupId = request.UserGroupData.GroupId,
        UserId = user.Id,
        RoleId = (await _roleManager.FindByNameAsync(request.UserGroupData.RoleName)).Id
      });
      _dbContext.SaveChanges();
      var dto = CustomMapper.Mapper.Map<UserDTO>(user);
      dto.RoleName = request.UserGroupData.RoleName;
      dto.GroupId = request.UserGroupData.GroupId;
      return dto;
    }

    public async Task<UserDTO> UpdateUserAsync(string userName, UpdateUserRequest request)
    {
      var user = await _userManager.FindByNameAsync(userName);
      CustomMapper.Mapper.Map<UpdateUserRequest, User>(request, user);
      _dbContext.SaveChanges();
      var dto = CustomMapper.Mapper.Map<UserDTO>(user);
      return dto;
    }

    public async Task<bool> DeleteUserAsync(string userName)
    {
      try
      {
        var user = await _userManager.FindByNameAsync(userName);
        _userManager.DeleteAsync(user);
        _dbContext.SaveChanges();
        return true;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public async Task<bool> ChangeRoleAsync(string userName, string roleName, string groupId)
    {
      try
      {
        var role = await _roleManager.FindByNameAsync(roleName);
        var user = await _userManager.FindByNameAsync(userName);
        var link = _dbContext.UserGroupRoles.First(x => x.UserId == user.Id && x.GroupId == groupId);
        link.RoleId = role.Id;
        _dbContext.SaveChanges();
        return true;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
