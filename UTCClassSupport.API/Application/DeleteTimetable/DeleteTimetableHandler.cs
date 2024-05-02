using MediatR;
using Microsoft.AspNetCore.Identity;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.DeleteTimetable
{
  public class DeleteTimetableHandler : IRequestHandler<DeleteTimetableCommand, DeleteTimetableResponse>
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    public DeleteTimetableHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
    }

    public Task<DeleteTimetableResponse> Handle(DeleteTimetableCommand request, CancellationToken cancellationToken)
    {
      var timetable = _dbContext.Timetables.Where(x => x.GroupId == request.GroupId && x.CreatedBy == request.UserName);
      _dbContext.Timetables.RemoveRange(timetable);
      _dbContext.SaveChanges();
      return Task.FromResult(new DeleteTimetableResponse()
      {
        Success = true,
        Message = "Delete success"
      });
    }
  }
}
