using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Common.Mail;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Utilities;

namespace UTCClassSupport.API.Application.UploadNewsToBulletin
{
  public class UploadNewsToBulletinHandler : IRequestHandler<UploadNewsToBulletinCommand, UploadNewsToBulletinResponse>
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly MailSettings _mailSettings;

    public UploadNewsToBulletinHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IOptions<MailSettings> mailSettings)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
      _mailSettings = mailSettings.Value;
    }
    public async Task<UploadNewsToBulletinResponse> Handle(UploadNewsToBulletinCommand request, CancellationToken cancellationToken)
    {
      var news = new Bulletin()
      {
        Content = request.Content,
        GroupId = request.GroupId,
        CreatedDate = DateTime.UtcNow,
        CreatedBy = request.UserName
      };
      NotificationProvider notificationProvider = new NotificationProvider();
      var group = _dbContext.Groups.Find(request.GroupId);
      if (request.RoleName == GroupRole.User.ToString())
      {
        news.Approved = (int)ApproveProcess.OnHold;
        // need send notify to manager (send mail/tel/notify in web)
        var managerRole = await _roleManager.FindByNameAsync(GroupRole.Manager.ToString());
        var managers = _dbContext.UserGroupRoles.Where(x => x.RoleId == managerRole.Id)
          .Include(x => x.User).Select(x => x.User).ToList();
        foreach (var manager in managers)
        {
          var noti = notificationProvider.CreateUserNotification(manager.Id, manager.DisplayName,
            request.UserName, request.DisplayName, Common.NotificationAction.PendingPost, news.Id);
          _dbContext.Notifications.Add(noti);
        }
        _dbContext.SaveChanges();
      }
      if (request.RoleName == GroupRole.Manager.ToString())
      {
        news.Approved = (int)ApproveProcess.Accept;
        var users = _dbContext.UserGroupRoles.Where(x => x.GroupId == request.GroupId).Include(x => x.User).Select(x => x.User);
        MailHandler mailHandler = new MailHandler(_mailSettings);
        foreach (var user in users)
        {
          if (user.EmailConfirmed)
          {
            // send mail to everyone in group
            mailHandler.Send(new MailContent()
            {
              To = user.Email,
              Subject = "Thông báo mới: " + request.Content.Substring(0, 32) + "...",
              Body = request.Content
            });
          }
        }
      }
      _dbContext.Bulletins.Add(news);
      // notify in web
      var notification = notificationProvider.CreateGroupNotification(group.Id, group.Name,
        request.UserName, request.DisplayName, Common.NotificationAction.NewPost, news.Id);
      _dbContext.Notifications.Add(notification);

      await _dbContext.SaveChangesAsync();
      return new UploadNewsToBulletinResponse()
      {
        Success = true,
        Type = ResponseType.Success,
        Message = request.RoleName == GroupRole.Manager.ToString() 
                  ? "Đăng tin thành công" 
                  : "Tin của bạn sẽ được đăng sau khi quản lý chấp nhận"
      };
    }
  }
}
