namespace UTCClassSupport.API.Requests.Pagination
{
  public interface IPagination
  {
    int PageSize { get; set; }
    int PageNumber { get; set; }
  }
}
