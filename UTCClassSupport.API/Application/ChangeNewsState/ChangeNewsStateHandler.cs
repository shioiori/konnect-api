using MediatR;
using Microsoft.AspNetCore.Identity;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.ChangeNewsState
{
  public class ChangeNewsStateHandler : IRequestHandler<ChangeNewsStateCommand, ChangeNewsStateResponse>
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    public ChangeNewsStateHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
    }

    public async Task<ChangeNewsStateResponse> Handle(ChangeNewsStateCommand request, CancellationToken cancellationToken)
    {
      var news = _dbContext.Bulletins.Find(request.PostId);
      if (news == null)
      {

      }
      news.Approved = (int)request.State;
      await _dbContext.SaveChangesAsync();
      return new ChangeNewsStateResponse();
    }
  }
}
