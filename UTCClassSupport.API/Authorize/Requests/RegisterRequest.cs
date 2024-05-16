namespace UTCClassSupport.API.Authorize.Requests
{
  public class RegisterRequest
  {
    public string DisplayName { get; set; }
    public string UserName { get; set; }
    public string Avatar { get; set; }
    public string? Password { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
  }

}
