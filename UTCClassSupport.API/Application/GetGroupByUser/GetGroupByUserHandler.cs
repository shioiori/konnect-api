using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.GetGroupByUser
{
  public class GetGroupByUserHandler : IRequestHandler<GetGroupByUserQuery, GetGroupByUserResponse>
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    public GetGroupByUserHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
    }
    public Task<GetGroupByUserResponse> Handle(GetGroupByUserQuery request, CancellationToken cancellationToken)
    {
      var group = _dbContext.UserGroupRoles.Where(x => x.UserId == request.UserId)
                .Include(x => x.Group).Select(x => x.Group).ToList();
      return Task.FromResult(new GetGroupByUserResponse()
      {
        Groups = CustomMapper.Mapper.Map<List<GroupDTO>>(group),
      });
    }
  }
}
