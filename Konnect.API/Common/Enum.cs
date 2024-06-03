namespace UTCClassSupport.API.Common
{
  public enum GroupRole
  {
    User = 1,
    Manager = 2,
  }

  public enum ApproveState
  {
    Pending = -1,
    Reject = 0,
    Accept = 1,
  }

  public enum FileTemplate
  {
    User,
    Timetable
  }

  public enum EventCategory
  {
    Unclassified,
    Timetable,
    Google,
    User,
  }

  public enum NotificationRange
  {
    All,
    Group,
    User
  }

  public enum NotificationAction
  {
    NewPost,
    PendingPost,
    AcceptPost,
    RejectPost,
    ReplyPost,
    Mention,
    InviteToGroup,
    KickFromGroup,
    ChangeRole
  }

  public enum AttachedType
  {
    Group,
    Post,
    Comment,
    None
  }
}
