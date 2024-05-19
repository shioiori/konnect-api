namespace UTCClassSupport.API.Responses
{
  public class LoginResponse
  {
  }

  public class AuthenticationResponse : Response
  {
    public int StatusCode { get; set; }
    public string AccessToken { get; set; }
  }
}
