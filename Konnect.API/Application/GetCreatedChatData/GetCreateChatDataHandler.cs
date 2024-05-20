using Konnect.API.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;

namespace Konnect.API.Application.GetCreatedChatData
{

  public class GetCreateChatDataHandler : IRequestHandler<GetCreateChatDataQuery, GetCreateChatData>
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    public GetCreateChatDataHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
    }

    public async Task<GetCreateChatData> Handle(GetCreateChatDataQuery request, CancellationToken cancellationToken)
    {
      var group = _dbContext.Groups.FirstOrDefault(x => x.Id == request.GroupId);
      var users = new List<UserData>();
      bool isIncludeUser = false;
      foreach (var username in request.UserNames)
      {
        var user = await _userManager.FindByNameAsync(username);
        if (username == request.UserName)
        {
          isIncludeUser = true;
        }
        users.Add(CustomMapper.Mapper.Map<UserData>(user));
      }
      if (!isIncludeUser)
      {
        var createdBy = await _userManager.FindByNameAsync(request.UserName);
        users.Add(CustomMapper.Mapper.Map<UserData>(createdBy));
      }
      var data = new GetCreateChatData() { 
        GroupData = CustomMapper.Mapper.Map<GroupData>(group),
        Users = users,
        CreatedBy = request.UserId
      };
      return data;
    }
  }
}
