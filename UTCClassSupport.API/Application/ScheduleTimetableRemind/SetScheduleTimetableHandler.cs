using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.ScheduleTimetableRemind
{
  public class SetScheduleTimetableHandler : IRequestHandler<SetScheduleTimetableCommand, SetScheduleTimetableResponse>
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly BackgroundJobServer _backgroundJobServer;
    public SetScheduleTimetableHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
    }
    public Task<SetScheduleTimetableResponse> Handle(SetScheduleTimetableCommand request, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
  }
}
