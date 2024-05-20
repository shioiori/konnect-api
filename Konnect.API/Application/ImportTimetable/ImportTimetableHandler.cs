using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Data;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.ImportTimetable
{
  public class ImportTimetableHandler : IRequestHandler<ImportTimetableCommand, ImportResponse>
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    public ImportTimetableHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
    }
    public async Task<ImportResponse> Handle(ImportTimetableCommand request, CancellationToken cancellationToken)
    {
      var datatable = ExcelHelper.ConvertExcelToDataTable(request.File, FileTemplate.Timetable);
      var timetable = _dbContext.Timetables
        .Where(x => x.GroupId == request.GroupId && x.CreatedBy == request.UserName)
        .FirstOrDefault();
      if (timetable == null)
      {
        timetable = new Timetable()
        {
          GroupId = request.GroupId,
          Url = "temp",
          CreatedDate = DateTime.Now,
          CreatedBy = request.UserName
        };
        _dbContext.Timetables.Add(timetable);
      }
      timetable.IsSynchronize = false;

      // datatable: stt - mahocphan - tenhocphan - so tinchi - lophocphan - ghe - thoigiandiadiem - hocphi

      foreach (DataRow row in datatable.Rows)
      {
        var text = row[6].ToString().Split("\n");
        var index = 0;
        while (text.Length > index)
        {
          var date = text[index].Split(new string[] { "Từ ", " đến ", ":" }, StringSplitOptions.RemoveEmptyEntries);
          index++;
          while (text.Length > index && text[index][0] == ' ')
          {
            var local = text[index].Split(new string[] { " Thứ ", " tiết ", " tại " }, StringSplitOptions.RemoveEmptyEntries);
            var periodTime = ShiftHelper.ConvertPeriodToTime(DateTime.Parse(date[0]), DateTime.Parse(date[1]), local[1]);
            var shift = new Event()
            {
              TimetableId = timetable.Id,
              Subject = row[2].ToString(),
              SubjectCode = row[1].ToString(),
              SubjectClass = row[4].ToString(),
              Credit = int.Parse(row[3].ToString()),
              From = DateTime.ParseExact(date[0], "dd/MM/yyyy", null),
              To = DateTime.ParseExact(date[1], "dd/MM/yyyy", null),
              PeriodStart = periodTime.StartTime.TimeOfDay,
              PeriodEnd = periodTime.EndTime.TimeOfDay,
              IsLoopPerDay = true,
              Day = int.Parse(local[0]),
              Location = local[2],
              Title = row[2].ToString(),
              Description = local[2],
              Category = EventCategory.Timetable,
            };
            //var availableShift = _dbContext.Shifts.FirstOrDefault(x => x.Code == shift.Code && x.Day == shift.Day
            //                                    && ((x.From >= shift.From && x.From <= shift.To)
            //                                    || (x.To >= shift.From && x.To <= shift.To)));
            //if (availableShift != null)
            //{
            //  _dbContext.Shifts.Remove(availableShift);
            //}
            _dbContext.Events.AddAsync(shift);
            index++;
          }
        }
      }
      await _dbContext.SaveChangesAsync();
      return new ImportResponse()
      {
        Success = true,
        Type = ResponseType.Success,
        Message = "Import thành công"
      };
    }
  }
}
