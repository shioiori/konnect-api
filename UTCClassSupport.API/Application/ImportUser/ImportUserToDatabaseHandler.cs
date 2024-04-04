using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Data;
using UTCClassSupport.API.Common;
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
    public ImportUserToDatabaseHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
    }

    // template: Id - Name - Email - Tel
    public async Task<ImportUserToDatabaseResponse> Handle(ImportUserToDatabaseCommand request, CancellationToken cancellationToken)
    {
      var dataTable = ExcelHelper.ConvertExcelToDataTable(request.FilePath);
      foreach (DataRow row in dataTable.Rows)
      {
        var user = new User()
        {
          UserName = row[0].ToString(),
          NormalizedUserName = row[1].ToString(),
          Email = row[2].ToString(),
          PhoneNumber = row[3].ToString(),
        };
        await _userManager.CreateAsync(user);
        _userManager.AddPasswordAsync(user, row[0].ToString());
        _dbContext.UserGroupRoles.Add(new UserGroupRole()
        {
          UserName = user.UserName,
          RoleName = GroupRole.User.ToString(),
          GroupId = request.GroupId
        });
      }
      return new ImportUserToDatabaseResponse();
    }
  }
}
