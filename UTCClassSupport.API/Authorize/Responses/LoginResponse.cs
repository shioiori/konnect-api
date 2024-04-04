namespace UTCClassSupport.API.Authorize.Responses
{
  public class LoginResponse
  {
  }

  public class AuthenticationResponse
  {
    public bool Success { get; set; }
    public string Message { get; set; }
    public int StatusCode { get; set; }
    public string AccessToken { get; set; }
  }
}
