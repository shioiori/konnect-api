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
      var datatable = ExcelHelper.ConvertExcelToDataTable(request.FilePath);
      var timetable = _dbContext.Timetables
        .Where(x => x.GroupId == request.GroupId && x.CreatedBy == "current user")
        .FirstOrDefault();
      if (timetable == null)
      {
        timetable = new Timetable()
        {
          GroupId = request.GroupId,
          CreatedDate = DateTime.Now,
          CreatedBy = "current user"
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
          var date = text[index].Split(new string[] { "Từ ", " đến " }, StringSplitOptions.None);
          if (index >= text.Length - 1) break;
          var local = text[index + 1].Split(new string[] { " Thứ ", " tiết ", " tại " }, StringSplitOptions.None);
          var shift = new Shift()
          {
            TimetableId = timetable.Id,
            Subject = row[2].ToString(),
            SubjectCode = row[1].ToString(),
            SubjectClass = row[4].ToString(),
            Credit = int.Parse(row[3].ToString()),
            From = DateTime.Parse(date[0]),
            To = DateTime.Parse(date[1]),
            Code = ShiftHelper.ConvertPeriodToShiftCode(local[1]),
            Day = int.Parse(local[0]),
            Location = local[2]
          };
          _dbContext.Shifts.AddAsync(shift);
          index++;
        }
      }
      await _dbContext.SaveChangesAsync();
      return new ImportTimetableResponse();
    }
  }
}
