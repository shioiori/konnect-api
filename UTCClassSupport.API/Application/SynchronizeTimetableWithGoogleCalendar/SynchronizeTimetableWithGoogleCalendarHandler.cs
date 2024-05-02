using MediatR;
using Microsoft.AspNetCore.Identity;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Responses.DTOs;

namespace UTCClassSupport.API.Application.ClearTimetable
{
  public class SynchronizeTimetableWithGoogleCalendarHandler : IRequestHandler<SynchronizeTimetableWithGoogleCalendarCommand, SynchronizeTimetableWithGoogleCalendarResponse>
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    public SynchronizeTimetableWithGoogleCalendarHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
    }
    public Task<SynchronizeTimetableWithGoogleCalendarResponse> Handle(SynchronizeTimetableWithGoogleCalendarCommand request, CancellationToken cancellationToken)
    {
      if (request.TimetableId == null)
      {
        var timetable = _dbContext.Timetables.FirstOrDefault(x => x.CreatedBy == request.UserName && x.GroupId == request.GroupId);
        if (timetable == null)
        {
          return Task.FromResult(new SynchronizeTimetableWithGoogleCalendarResponse()
          {
            Success = true,
            Message = "User don't have any timetable"
          });
        }
        request.TimetableId = timetable.Id;
      }
      var shifts = _dbContext.Shifts.Where(x => x.TimetableId == request.TimetableId);
      _dbContext.Shifts.RemoveRange(shifts);

      var timetable2 = _dbContext.Timetables.First(x => x.Id == request.TimetableId);
      timetable2.IsSynchronize = true;
      _dbContext.SaveChanges();
      
      return Task.FromResult(new SynchronizeTimetableWithGoogleCalendarResponse()
      {
        Success = true,
        Message = "Success",
      });
    }
  }
}
