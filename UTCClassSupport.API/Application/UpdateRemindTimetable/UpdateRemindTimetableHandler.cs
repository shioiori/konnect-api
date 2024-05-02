using MediatR;
using Microsoft.AspNetCore.Identity;
using UTCClassSupport.API.Application.UpdateTimetable;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.UpdateRemindTimetable
{
  public class UpdateRemindTimetableHandler : IRequestHandler<UpdateRemindTimetableCommand, UpdateRemindTimetableResponse>
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    public UpdateRemindTimetableHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
    }
    public Task<UpdateRemindTimetableResponse> Handle(UpdateRemindTimetableCommand request, CancellationToken cancellationToken)
    {
      var timetable = _dbContext.Timetables.FirstOrDefault(x => x.GroupId == request.GroupId && x.CreatedBy == request.UserName);
      if (timetable == null)
      {
        return Task.FromResult(new UpdateRemindTimetableResponse()
        {
          Success = false,
          Message = "Người dùng này không có thời khoá biểu"
        });
      }
      timetable.Remind = request.RemindTime;
      _dbContext.SaveChanges();
      return Task.FromResult(new UpdateRemindTimetableResponse()
      {
        Success = true,
        Message = "Cập nhật thành công"
      });
    }
  }
}
