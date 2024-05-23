using DocumentFormat.OpenXml.InkML;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using System.Data;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Common.Mail;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.ImportExcel
{
  public class ImportUserToDatabaseHandler : IRequestHandler<ImportUserToDatabaseCommand, ImportUserToDatabaseResponse>
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    private readonly MailSettings _mailSettings;
    public ImportUserToDatabaseHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IOptions<MailSettings> mailSettings)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
      _mailSettings = mailSettings.Value;
    }

    // template: Id - Name - Email - Tel
    public async Task<ImportUserToDatabaseResponse> Handle(ImportUserToDatabaseCommand request, CancellationToken cancellationToken)
    {
      if (!ExcelHelper.IsExcelFile(request.File.FileName))
      {
        return new ImportUserToDatabaseResponse()
        {
          Success = false,
          Message = "Your file is not excel"
        };
      }
      var dataTable = ExcelHelper.ConvertExcelToDataTable(request.File, FileTemplate.User);
      var role = await _roleManager.FindByNameAsync(GroupRole.User.ToString());
      if (role == null)
      {
        role = new Role()
        {
          Name = GroupRole.User.ToString()
        };
        await _roleManager.CreateAsync(role);
      }
      using (var transaction = _dbContext.Database.BeginTransaction())
      {
        try
        {
          MailHandler mailHandler = new MailHandler(_mailSettings);
          List<MailContent> mailContents = new List<MailContent>();
          foreach (DataRow row in dataTable.Rows)
          {
            var existUser = await _userManager.FindByNameAsync(row[0].ToString());
            if (existUser != default)
            {
              var inGroup = _dbContext.UserGroupRoles.FirstOrDefault(x => x.UserId == existUser.Id && x.GroupId == request.GroupId);
              if (inGroup != null)
              {
                continue;
              }  
              else
              {
                mailContents.Add(new MailContent()
                {
                  To = existUser.Email,
                  Subject = GetInviteSubject(existUser.DisplayName, request.GroupId),
                  Body = GetInviteContent(existUser.DisplayName, request.GroupId)
                });
                continue;
              }
            }
            var user = new User()
            {
              UserName = row[0].ToString(),
              DisplayName = row[1].ToString(),
              Email = row[2].ToString(),
              PhoneNumber = row[3].ToString(),
            };
            await _userManager.CreateAsync(user);
            _dbContext.SaveChanges();
            await _userManager.AddToRoleAsync(user, role.Name);
            await _userManager.AddPasswordAsync(user, row[0].ToString());
            _dbContext.UserGroupRoles.Add(new UserGroupRole()
            {
              UserId = user.Id,
              RoleId = role.Id,
              GroupId = request.GroupId
            });
            _dbContext.SaveChanges();
          }
          transaction.Commit();
          foreach (var mail in mailContents)
          {
            mailHandler.Send(mail);
          }
        }
        catch (Exception ex)
        {
          transaction.Rollback();
          return new ImportUserToDatabaseResponse()
          {
            Success = false,
            Type = ResponseType.Error,
            Message = "Có lỗi trong quá trình import, hãy kiểm tra lại file"
        };
      }
      }
      return new ImportUserToDatabaseResponse()
      {
        Success = true,
        Type = ResponseType.Success,
        Message = "Import thành công"
      };
    }

    private string GetInviteSubject(string displayName, string groupId)
    {
      return $"{displayName} thân mến, bạn có một lời mời";
    }

    private string GetInviteContent(string displayName, string groupId)
    {
      return $"Quản lý viên mời bạn tham gia group {groupId}.<br/> " +
        $"Nếu bạn đồng ý, hãy đăng nhập rồi nhấn vào " +
        $"<a href='{AppSetting.Host}/group/join/{groupId}'>đây</a><br/>" +
        $"Nếu không phải, xin hãy bỏ qua tin nhắn này. " +
        $"<br/><br/>Trân trọng cảm ơn." +
        $"<br/>{AppSetting.AppName}";
    }
  }
}
