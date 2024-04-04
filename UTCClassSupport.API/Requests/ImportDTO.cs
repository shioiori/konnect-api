namespace UTCClassSupport.API.Requests
{
  public class ImportUserDTO
  {
    public string GroupId { get; set; }
    public IFormFile File { get; set; }
  }
}
