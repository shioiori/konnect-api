namespace UTCClassSupport.API.Requests
{
  public class UserRequest
  {
    public string UserName { get; set; }
    public string DisplayName { get; set; }
    public string Avatar { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
  }
    public class AddUserRequest
  {
    public string UserName { get; set; }
    public string DisplayName { get; set; }
    public string Avatar { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public UserGroupData? UserGroupData { get; set; }
  }
  public class UpdateUserRequest
  {
    public string DisplayName { get; set; }
    public string Avatar { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
  }

  public class JoinRequest
  {
    public string GroupId { get; set; }
    public string Email { get; set; }
  }
}
