using MediatR;
using Microsoft.AspNetCore.Identity;
using UTCClassSupport.API.Common;
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
      var timetable = _dbContext.Timetables.FirstOrDefault(x => x.GroupId == request.GroupId && x.CreatedBy == request.UserName);
      if (timetable == null)
      {
        return Task.FromResult(new DeleteTimetableResponse()
        {
          Success = true,
          Type = ResponseType.Success,
          Message = "Không tồn tại thời khoá biểu"
        });
      }
      var events = _dbContext.Events.Where(x => x.TimetableId == timetable.Id);
      _dbContext.Events.RemoveRange(events);
      _dbContext.SaveChanges();
      return Task.FromResult(new DeleteTimetableResponse()
      {
        Success = true,
        Type = ResponseType.Success,
        Message = "Xóa thành công"
      });
    }
  }
}
