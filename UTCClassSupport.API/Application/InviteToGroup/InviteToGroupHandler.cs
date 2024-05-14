using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Common.Mail;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Utilities;

namespace UTCClassSupport.API.Application.InviteToGroup
{
  public class InviteToGroupHandler : IRequestHandler<InviteToGroupCommand, InviteToGroupResponse>
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly MailSettings _mailSettings;
    public InviteToGroupHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IOptions<MailSettings> mailSettings)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
      _mailSettings = mailSettings.Value;
    }
    public async Task<InviteToGroupResponse> Handle(InviteToGroupCommand request, CancellationToken cancellationToken)
    {
      if (request.IsExistUser)
      {
        var guest = await _userManager.FindByNameAsync(request.Guest);
        if (guest != null)
        {
          _dbContext.UserGroupRoles.Add(new UserGroupRole()
          {
            UserId = guest.Id,
            GroupId = request.GroupId,
            RoleId = (await _roleManager.FindByNameAsync(GroupRole.User.ToString())).Id,
          });
          // notify
        }
        else
        {
          return new InviteToGroupResponse(){
            Message = "Người dùng này không tồn tại",
            Success = false,
            Type = ResponseType.Error
          };
        }
      }
      else
      {
        var user = await _userManager.FindByEmailAsync(request.Guest);
        if (user != default)
        {
          return new InviteToGroupResponse()
          {
            Message = "Đã có người sử dụng email này",
            Success = false,
            Type = ResponseType.Error
          };
        }
        else
        {
          MailHandler mailHandler = new MailHandler(_mailSettings);
          var group = _dbContext.Groups.First(x => x.Id == request.GroupId);
          JoinRequest joinRequest = new JoinRequest()
          {
            GroupId = request.GroupId,
            Email = request.Guest,
          };
          var key = AesOperation.GenerateKey();
          var encryptText = AesOperation.EncryptString(key, JsonConvert.SerializeObject(joinRequest));
          var token = AesOperation.GenerateToken(key, encryptText);
          mailHandler.Send(new MailContent()
          {
            To = request.Guest,
            Subject = GetInviteMailSubject(request.DisplayName),
            Body = GetInviteMailContent(request.DisplayName, group.Name, token)
          });
        }
      }
      return new InviteToGroupResponse()
      {
        Message = "Gửi lời mời thành công",
        Success = true,
        Type = ResponseType.Success
      };
    }

    public string GetInviteMailSubject(string sender)
    {
      return "Bạn có một lời mời từ " + sender;
    }

    private string GetInviteMailContent(string sender, string groupName, string accessCode)
    {
      return $"Thân mến,\n{sender} mời bạn tham gia group {groupName}. Nếu bạn đồng ý, nhấn vào đường link dưới đây\n {accessCode}\n để nhận lời mời.";
    }
  }
}
