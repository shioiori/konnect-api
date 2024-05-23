using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Common.Mail;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;

namespace UTCClassSupport.API.Utilities
{
  public class NotificationProvider
  {
    public Notification CreateGroupNotification(string groupId, string groupName,
      string createdBy, string createdName, NotificationAction action, string? id)
    {
      switch (action)
      {
        case NotificationAction.NewPost:
          return new Notification()
          {
            Content = $"{createdName} vừa đăng một tin mới",
            Action = action,
            Range = NotificationRange.Group,
            ObjectId = id,
            GroupId = groupId,
            CreatedDate = DateTime.Now,
            CreatedBy = createdBy,
          };
        default:
          return null;
      }
    }

    public Notification CreateUserNotification(string receiverId, string receiverName,
      string createdBy, string createdName, NotificationAction action, string? id)
    {
      switch (action)
      {
        case NotificationAction.ReplyPost:
          return new Notification()
          {
            Content = $"{receiverName} vừa trả lời bài đăng của bạn",
            Action = action,
            Range = NotificationRange.User,
            ObjectId = id,
            UserId = receiverId,
            CreatedDate = DateTime.Now,
            CreatedBy = createdBy,
          };
        case NotificationAction.PendingPost:
          return new Notification()
          {
            Content = $"Có một tin chờ duyệt đến từ {createdName}",
            Action = action,
            Range = NotificationRange.User,
            ObjectId = id,
            UserId = receiverId,
            CreatedDate = DateTime.Now,
            CreatedBy = createdBy,
          };
        case NotificationAction.AcceptPost:
          return new Notification()
          {
            Content = $"{createdName} đã duyệt tin của bạn",
            Action = action,
            Range = NotificationRange.User,
            ObjectId = id,
            UserId = receiverId,
            CreatedDate = DateTime.Now,
            CreatedBy = createdBy,
          };
        case NotificationAction.KickFromGroup:
          return new Notification()
          {
            Content = $"Bạn đã rời khỏi group {id}",
            Action = action,
            Range = NotificationRange.User,
            ObjectId = id,
            UserId = receiverId,
            CreatedDate = DateTime.Now,
            CreatedBy = createdBy,
          };
        case NotificationAction.InviteToGroup:
          return new Notification()
          {
            Content = $"Bạn được mời vào group {id}",
            Action = action,
            Range = NotificationRange.User,
            ObjectId = id,
            UserId = receiverId,
            CreatedDate = DateTime.Now,
            CreatedBy = createdBy,
          };
        default:
          return null;
      }
    }

    public AttachedType GetAttachedType(NotificationAction action)
    {
      switch (action)
      {
        case NotificationAction.NewPost:
        case NotificationAction.PendingPost:
        case NotificationAction.AcceptPost:
        case NotificationAction.ReplyPost:
          return AttachedType.Post;
        case NotificationAction.InviteToGroup:
        case NotificationAction.KickFromGroup:
        case NotificationAction.ChangeRole:
          return AttachedType.Group;
        case NotificationAction.ReplyComment:
          return AttachedType.Comment;
        default:
          return AttachedType.None;
      }
    }
  }
}
