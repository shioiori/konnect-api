namespace UTCClassSupport.API.Requests
{
  public class UserRequest
  {
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Role { get; set; }
    public string? Password { get; set; }
  }

  public class JoinRequest
  {
    public string GroupId { get; set; }
    public string Email { get; set; }
  }
}
