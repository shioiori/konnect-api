namespace UTCClassSupport.API.Responses.DTOs
{
  public class UserDTO
  {
    public string Id { get; set; }
    public string UserName { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string RoleName { get; set; }
    public string? GroupId { get; set; }
    public string Avatar { get; set; }
  }

    public class RoleDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
