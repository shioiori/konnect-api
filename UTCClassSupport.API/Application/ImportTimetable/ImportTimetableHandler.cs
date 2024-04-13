using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Data;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.ImportTimetable
{
  public class ImportTimetableHandler : IRequestHandler<ImportTimetableCommand, ImportTimetableResponse>
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
    public async Task<ImportTimetableResponse> Handle(ImportTimetableCommand request, CancellationToken cancellationToken)
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
      else
      {
        // delete all timetable and shift (reference: cascade)
        var shifts = _dbContext.Shifts.Where(x => x.TimetableId == timetable.Id);
        _dbContext.Shifts.RemoveRange(shifts);
      }

      // datatable: stt - mahocphan - tenhocphan - so tinchi - lophocphan - ghe - thoigiandiadiem - hocphi

      foreach (DataRow row in datatable.Rows)
      {
        var text = row[6].ToString().Split("\n");
        var index = 0;
        while (text.Length > index)
        {
          var date = text[index].Split(new string[] { "Từ ", " đến ", ":" }, StringSplitOptions.RemoveEmptyEntries);
          index++;
          if (text.Length <= index) break;
          while (text[index][0] == ' ')
          {
            var local = text[index].Split(new string[] { " Thứ ", " tiết ", " tại " }, StringSplitOptions.RemoveEmptyEntries);

            var shift = new Shift()
            {
              TimetableId = timetable.Id,
              Subject = row[2].ToString(),
              SubjectCode = row[1].ToString(),
              SubjectClass = row[4].ToString(),
              Credit = int.Parse(row[3].ToString()),
              From = DateTime.ParseExact(date[0], "dd/MM/yyyy", null),
              To = DateTime.ParseExact(date[1], "dd/MM/yyyy", null),
              Code = ShiftHelper.ConvertPeriodToShiftCode(local[1]),
              Day = int.Parse(local[0]),
              Location = local[2]
            };
            var availableShift = _dbContext.Shifts.FirstOrDefault(x => x.Code == shift.Code && x.Day == shift.Day
                                                && ((x.From >= shift.From && x.From <= shift.To)
                                                || (x.To >= shift.From && x.To <= shift.To)));
            if (availableShift != null)
            {
              _dbContext.Shifts.Remove(availableShift);
            }
            _dbContext.Shifts.AddAsync(shift);
            index++;
          }
        }
      }
      await _dbContext.SaveChangesAsync();
      return new ImportTimetableResponse();
    }
  }
}
