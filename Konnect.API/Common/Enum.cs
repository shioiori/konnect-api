namespace UTCClassSupport.API.Common
{
  public enum GroupRole
  {
    User = 1,
    Manager = 2,
  }

  public enum ApproveProcess
  {
    OnHold = -1,
    Deny = 0,
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
