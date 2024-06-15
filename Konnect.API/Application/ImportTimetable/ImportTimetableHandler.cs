using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Data;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Responses.DTOs;

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
			using (var transaction = _dbContext.Database.BeginTransaction())
			{
				try
				{
					var datatable = ExcelHelper.ConvertExcelToDataTable(request.File, FileTemplate.Timetable);
					var timetable = _dbContext.Timetables
					  .Where(x => x.CreatedBy == request.UserName)
					  .FirstOrDefault();
					if (timetable == null)
					{
						timetable = new Timetable()
						{
							CreatedBy = request.UserName,
							Remind = -1,
							IsSynchronize = false,
						};
						_dbContext.Timetables.Add(timetable);
					}
					List<Event> events = new List<Event>();
					int create = 0, ignore = 0;
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
								var from = DateTime.ParseExact(date[0], "dd/MM/yyyy", null);
								var to = DateTime.ParseExact(date[1], "dd/MM/yyyy", null);
								var periodTime = ShiftHelper.ConvertPeriodToTime(from, to, local[1]);
								var shift = new Event()
								{
									TimetableId = timetable.Id,
									Subject = row[2].ToString(),
									SubjectCode = row[1].ToString(),
									SubjectClass = row[4].ToString(),
									Credit = int.Parse(row[3].ToString()),
									From = from,
									To = to,
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
								index++;
								if (!timetable.IsSynchronize && _dbContext.Events.Count(x => x.TimetableId == timetable.Id
									&& x.Category == shift.Category
									&& x.From == shift.From && x.To == shift.To && x.Day == shift.Day) == 0)
								{
									events.Add(shift);
									create++;
								}
								else ignore++;
							}
						}
					}
					if (!timetable.IsSynchronize)
					{
						_dbContext.Events.AddRange(events);
						_dbContext.SaveChanges();
						transaction.Commit();
					}
					else
					{
						return new ImportTimetableResponse()
						{
							Success = true,
							Type = ResponseType.Success,
							Message = "Import thành công",
							Events = CustomMapper.Mapper.Map<List<EventDTO>>(events)
						};
					}
					return new ImportResponse()
					{
						Success = true,
						Type = ResponseType.Success,
						Message = $"Import thành công. Thêm mới {create} sự kiện và bỏ qua {ignore} sự kiện"
					};
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					return new ImportResponse()
					{
						Success = false,
						Type = ResponseType.Error,
						Message = ex.Message
					};
				}
			}
		}
	}
}
