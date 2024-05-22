using MediatR;
using Microsoft.AspNetCore.Identity;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Responses.DTOs;

namespace UTCClassSupport.API.Application.GetUserTimetable
{
    public class GetUserTimetableHandler : IRequestHandler<GetUserTimetableQuery, GetUserTimetableResponse>
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    public GetUserTimetableHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
    }
    public Task<GetUserTimetableResponse> Handle(GetUserTimetableQuery request, CancellationToken cancellationToken)
    {
      try {
        var timetable = _dbContext.Timetables.FirstOrDefault(x => x.CreatedBy == request.UserName
                                                              && x.GroupId == request.GroupId);
        if (timetable == default)
        {
          timetable = new Timetable() { 
            GroupId = request.GroupId,
            IsSynchronize = false,
            Remind = -1,
            CreatedDate = DateTime.Now,
            CreatedBy = request.UserName,
          };
          _dbContext.Timetables.Add(timetable);
          _dbContext.SaveChanges();
        }
        var shifts = _dbContext.Events.Where(x => x.TimetableId == timetable.Id).ToList();
        DateTime start = DateTime.MaxValue, end = DateTime.MinValue;
        foreach (var shift in shifts)
        {
          start = start > shift.From ? shift.From : start;
          end = end < shift.To ? shift.To : end;
        }
        return Task.FromResult(new GetUserTimetableResponse()
        {
          From = start,
          To = end,
          IsSynchronize = timetable.IsSynchronize,
          RemindTime = timetable.Remind,
          Events = CustomMapper.Mapper.Map<List<EventDTO>>(shifts)
        });
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }  
  }
}
