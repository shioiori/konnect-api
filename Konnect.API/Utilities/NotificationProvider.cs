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
						Content = $"<a class='comment-mention'>{createdName}</a> vừa đăng một tin mới",
						Action = action,
						Range = NotificationRange.Group,
						ObjectId = id,
						GroupId = groupId,
						CreatedDate = DateTime.Now,
						CreatedBy = createdBy,
					};
				case NotificationAction.ChangeRole:
					return new Notification()
					{
						Content = $"<a class='comment-mention'>{createdName}</a> đã trở thành Quản lý của nhóm <a class='comment-mention'>{groupName}</a>",
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
		  string createdBy, string createdName, NotificationAction action, string? id, string message = "")
		{
			if (createdBy == String.Empty)
			{
				createdBy = "system";
				createdName = "Hệ thống";
			}
			switch (action)
			{
				case NotificationAction.ReplyPost:
					return new Notification()
					{
						Content = $"<a class='comment-mention'>{createdName}</a> vừa trả lời bài đăng của bạn",
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
						Content = $"Có một tin chờ duyệt đến từ <a class='comment-mention'>{createdName}</a>",
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
						Content = $"<a class='comment-mention'>{createdName}</a> đã duyệt tin của bạn",
						Action = action,
						Range = NotificationRange.User,
						ObjectId = id,
						UserId = receiverId,
						CreatedDate = DateTime.Now,
						CreatedBy = createdBy,
					};
				case NotificationAction.RejectPost:
					return new Notification()
					{
						Content = $"<a class='comment-mention'>{createdName}</a> đã từ chối tin của bạn với lý do: {message}",
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
						Content = $"Bạn đã rời khỏi group {message}",
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
						Content = $"Bạn được mời vào group {message}",
						Action = action,
						Range = NotificationRange.User,
						ObjectId = id,
						UserId = receiverId,
						CreatedDate = DateTime.Now,
						CreatedBy = createdBy,
					};
				case NotificationAction.ChangeRole:
					return new Notification()
					{
						Content = $"<a class='comment-mention'>{createdName}</a> thay đổi chức vụ của bạn thành {message}",
						Action = action,
						Range = NotificationRange.User,
						ObjectId = id,
						UserId = receiverId,
						CreatedDate = DateTime.Now,
						CreatedBy = createdBy,
					};
				case NotificationAction.Mention:
					return new Notification()
					{
						Content = $"<a class='comment-mention'>{createdName}</a> đã nhắc tới bạn trong một bình luận",
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
				case NotificationAction.RejectPost:
					return AttachedType.Post;
				case NotificationAction.InviteToGroup:
				case NotificationAction.KickFromGroup:
				case NotificationAction.ChangeRole:
					return AttachedType.Group;
				case NotificationAction.Mention:
					return AttachedType.Comment;
				default:
					return AttachedType.None;
			}
		}
	}
}
