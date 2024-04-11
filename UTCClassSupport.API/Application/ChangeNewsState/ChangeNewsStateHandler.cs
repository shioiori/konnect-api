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
        return new ChangeNewsStateResponse()
        {
          Success = false,
          Message = "Can't find news with current id"
        };
      }
      var alert = news.Approved != (int)ApproveProcess.Accept ? true : false; 
      news.Approved = (int)request.State;
      await _dbContext.SaveChangesAsync();
      if (request.State == ApproveProcess.Accept && alert)
      {
        // notify to other
      }
      return new ChangeNewsStateResponse()
      {
        Success = true,
        Message = "You news' state has change"
      };
    }
  }
}
