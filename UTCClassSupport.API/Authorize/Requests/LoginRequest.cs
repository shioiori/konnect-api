namespace UTCClassSupport.API.Authorize.Requests
{
  public class LoginRequest
  {
    public string Username { get; set; }
    public string Password { get; set; }
    public bool IsLogin { get; set; }
  }
}
