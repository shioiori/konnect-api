using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.ScheduleTimetableRemind
{
  public class AddEventHandler : IRequestHandler<AddEventCommand, AddEventResponse>
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    public AddEventHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
    }
    public Task<AddEventResponse> Handle(AddEventCommand request, CancellationToken cancellationToken)
    {
      var timetable = _dbContext.Timetables.FirstOrDefault(x => x.GroupId == request.GroupId && x.CreatedBy == request.UserName);
      if (timetable == null)
      {
        return Task.FromResult(new AddEventResponse()
        {
          Success = false,
          Type = ResponseType.Error,
          Message = "Người dùng không có thời khóa biểu"
        });
      }
      var shift = new Event()
      {
        From = request.From,
        To = request.To,
        Title = request.Title,
        Description = request.Description,
        Location = request.Location,
        IsLoopPerDay = false,
        TimetableId = timetable.Id,
      };
      _dbContext.Events.Add(shift);
      _dbContext.SaveChanges();
      return Task.FromResult(new AddEventResponse()
      {
        Success = true,
        Type = ResponseType.Success,
        Message = "Thêm sự kiện thành công"
      });
    }
  }
}
