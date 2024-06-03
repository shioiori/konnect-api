﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Common.Mail;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests.Pagination;
using UTCClassSupport.API.Responses.DTOs;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Utilities;
using Konnect.API.Data;
using Microsoft.EntityFrameworkCore;

namespace UTCClassSupport.API.Infrustructure.Repositories
{
  public class NotificationManager
  {
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly EFContext _dbContext;
    private readonly IMailHandler _mailHandler;
    private readonly NotificationProvider _notificationProvider;

    public NotificationManager(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IConfiguration configuration,
        IMailHandler mailHandler,
        EFContext dbContext)
    {
      _userManager = userManager;
      _roleManager = roleManager;
      _configuration = configuration;
      _dbContext = dbContext;
      _mailHandler = mailHandler;
      _notificationProvider = new NotificationProvider();
    }

    public async Task<List<Notification>> GetNotificationAsync(string groupId, string userId, PaginationData paginationData)
    {
      var user = await _userManager.FindByIdAsync(userId);
      var notifications = _dbContext.Notifications.Where(x => x.Range == NotificationRange.All
                                   || (x.Range == NotificationRange.Group && x.GroupId == groupId 
                                                  && (user == default || x.CreatedBy != user.UserName))
                                   || (x.Range == NotificationRange.User && x.UserId == userId))
                                   .OrderByDescending(x => x.CreatedDate);
      if (paginationData == null)
      {
        return notifications.ToList();
      }
      var skip = (paginationData.PageNumber - 1) * paginationData.PageSize;
      var notis = notifications.Skip(skip).Take(paginationData.PageSize);
      return notis.ToList();
    }

    public void UpdateStateNotification(string groupId, string userId)
    {
      var notifications = _dbContext.Notifications.Where(x => x.IsSeen == false)
                              .Where(x => x.Range == NotificationRange.All 
                              || (x.Range == NotificationRange.Group && x.GroupId == groupId)
                              || (x.Range == NotificationRange.User && x.UserId == userId));
      foreach (var notification in notifications)
      {
        notification.IsSeen = true;
      }
      _dbContext.SaveChanges();
    }

    public object GetAttactedObject(string id, NotificationAction action)
    {
      var type = _notificationProvider.GetAttachedType(action);
      switch (type)
      {
        case AttachedType.Group:
          return _dbContext.Groups.FirstOrDefault(x => x.Id == id);
        case AttachedType.Post:
          return _dbContext.Bulletins.FirstOrDefault(x => x.Id == id);
        case AttachedType.Comment:
          return _dbContext.Comments.FirstOrDefault(x => x.Id == id);
        default:
          return null;
      }
    }

    public async Task NotifyNewCommentAsync(Bulletin post, UserInfo request)
    {
      if (request.UserName != post.CreatedBy)
      {
        var receiver = await _userManager.FindByNameAsync(post.CreatedBy);
        var notification = _notificationProvider.CreateUserNotification(receiver.Id, receiver.DisplayName,
          request.UserName, request.DisplayName, Common.NotificationAction.ReplyPost, post.Id);
        _dbContext.Notifications.Add(notification);
        _dbContext.SaveChanges();
      }
    }

    public void NotifyNewPost(Bulletin post, Group group, UserInfo request)
    {
      var notification = _notificationProvider.CreateGroupNotification(request.GroupId, group.Name,
        request.UserName, request.DisplayName, Common.NotificationAction.NewPost, post.Id);
      _dbContext.Notifications.Add(notification);
      _dbContext.SaveChanges();
    }
    public async Task NotifyPostAcceptAsync(Bulletin post, UserInfo request, string message)
    {
      try
      {
        if (request.UserName != post.CreatedBy)
        {
          var receiver = await _userManager.FindByNameAsync(post.CreatedBy);
          var notification = _notificationProvider.CreateUserNotification(receiver.Id, receiver.DisplayName,
            request.UserName, request.DisplayName, Common.NotificationAction.AcceptPost, post.Id, message);
          _dbContext.Notifications.Add(notification);
          _dbContext.SaveChanges();
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public async Task NotifyPostRejectAsync(Bulletin post, UserInfo request, string message)
    {
      if (request.UserName != post.CreatedBy)
      {
        var receiver = await _userManager.FindByNameAsync(post.CreatedBy);
        var notification = _notificationProvider.CreateUserNotification(receiver.Id, receiver.DisplayName,
          request.UserName, request.DisplayName, Common.NotificationAction.RejectPost, post.Id, message);
        _dbContext.Notifications.Add(notification);
        _dbContext.SaveChanges();
      }
    }

    public async Task NotifyPostPendingAsync(Bulletin post, UserInfo request)
    {
      try
      {
        var managerRole = await _roleManager.FindByNameAsync(GroupRole.Manager.ToString());
        var managers = _dbContext.UserGroupRoles.Where(x => x.GroupId == request.GroupId && x.RoleId == managerRole.Id)
          .Include(x => x.User).Select(x => x.User).ToList();
        foreach (var manager in managers)
        {
          var noti = _notificationProvider.CreateUserNotification(manager.Id, manager.DisplayName,
            request.UserName, request.DisplayName, Common.NotificationAction.PendingPost, post.Id);
          _dbContext.Notifications.Add(noti);
        }
        _dbContext.SaveChanges();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
