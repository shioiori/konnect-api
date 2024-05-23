using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Common.Mail;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests.Pagination;
using UTCClassSupport.API.Responses.DTOs;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Utilities;

namespace UTCClassSupport.API.Infrustructure.Repositories
{
  public class NotificationManager
  {
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly EFContext _dbContext;
    private readonly MailSettings _mailSettings;
    private readonly NotificationProvider _notificationProvider;

    public NotificationManager(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IConfiguration configuration,
        IOptions<MailSettings> mailSettings,
        EFContext dbContext)
    {
      _userManager = userManager;
      _roleManager = roleManager;
      _configuration = configuration;
      _dbContext = dbContext;
      _mailSettings = mailSettings.Value;
      _notificationProvider = new NotificationProvider();
    }

    public async Task<List<Notification>> GetNotificationAsync(string groupId, string userId, PaginationData paginationData)
    {
      var user = await _userManager.FindByIdAsync(userId);
      var notifications = _dbContext.Notifications.Where(x => x.Range == NotificationRange.All
                                   || (x.Range == NotificationRange.Group && x.GroupId == groupId 
                                                  && (user == default || x.CreatedBy != user.Id))
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
  }
}
