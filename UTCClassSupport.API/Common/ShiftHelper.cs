namespace UTCClassSupport.API.Common
{
  public class ShiftHelper
  {
    public static int ConvertPeriodToShiftCode(string period)
    {
      switch (period)
      {
        case "1,2,3":
          return 1;
        case "4,5,6":
          return 2;
        case "7,8,9":
          return 3;
        case "10,11,12":
          return 4;
      }
      return 0;
    }

    public static PeriodTime ConvertPeriodToTime(DateTime from, DateTime to, string period)
    {
      var periodTime = new PeriodTime();
      switch (period)
      {
        case "1,2,3":
          periodTime.StartTime = from.Date + TimeSpan.Parse("07:00:00");
          periodTime.EndTime = to.Date + TimeSpan.Parse("09:00:00");
          break; 
        case "4,5,6":
          periodTime.StartTime = from.Date + TimeSpan.Parse("09:30:00");
          periodTime.EndTime = to.Date + TimeSpan.Parse("11:30:00");
          break;
        case "7,8,9":
          periodTime.StartTime = from.Date + TimeSpan.Parse("13:00:00");
          periodTime.EndTime = to.Date + TimeSpan.Parse("15:00:00");
          break;
        case "10,11,12":
          periodTime.StartTime = from.Date + TimeSpan.Parse("15:30:00");
          periodTime.EndTime = to.Date + TimeSpan.Parse("17:30:00");
          break;
        default:
          periodTime.StartTime = from;
          periodTime.EndTime = to;
          break;
      }
      return periodTime;
    }
  }

  public class PeriodTime
  {
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
  }
}
