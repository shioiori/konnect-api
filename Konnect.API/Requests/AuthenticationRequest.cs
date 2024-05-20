namespace UTCClassSupport.API.Requests
{
  public class LoginRequest
  {
    public string Username { get; set; }
    public string Password { get; set; }
    public bool IsLogin { get; set; }
  }

  public class RegisterRequest
  {
    public string DisplayName { get; set; }
    public string UserName { get; set; }
    public string? Avatar { get; set; }
    public string? Password { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
  }

  public class ChangePasswordRequest
  {
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string NewPasswordConfirm { get; set; }
  }
}
