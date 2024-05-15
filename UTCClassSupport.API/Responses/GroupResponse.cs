using UTCClassSupport.API.Requests;

namespace UTCClassSupport.API.Responses
{
  public class GetGroupByUserResponse : Response
  {
    public List<GroupDTO> Groups { get; set; }
  }

  public class OutGroupResponse : Response { }

  public class JoinGroupResponse : Response { 
    public string Email { get; set; }
    public string GroupId { get; set; }
  }

  public class InviteToGroupResponse : Response { }
}
