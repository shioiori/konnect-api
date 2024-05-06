using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Common.Mail;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;

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
      if (request.RoleName == GroupRole.User.ToString())
      {
        news.Approved = (int)ApproveProcess.OnHold;
        // need send notify to manager (send mail/tel/notify in web)
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
