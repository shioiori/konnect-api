using UTCClassSupport.API.Responses.DTOs;

namespace UTCClassSupport.API.Responses
{
  public class GetUserResponse : Response
  {
    public UserDTO User { get; set; }
  }

  public class GetUsersResponse : Response
  {
    public List<UserDTO> Users { get; set; }
  }

  public class AddUserResponse : Response
  {
    public UserDTO User { get; set; }
  }

  public class UpdateUserResponse : Response { public UserDTO User { get; set;} }
  public class DeleteUserResponse : Response
  {
  }

  public class ChangeRoleResponse : Response { }
}
