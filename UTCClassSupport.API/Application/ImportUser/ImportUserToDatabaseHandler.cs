using DocumentFormat.OpenXml.InkML;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
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
      if (!ExcelHelper.IsExcelFile(request.File.FileName))
      {
        return new ImportUserToDatabaseResponse()
        {
          Success = false,
          Message = "Your file is not excel"
        };
      }
      var dataTable = ExcelHelper.ConvertExcelToDataTable(request.File);
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

          foreach (DataRow row in dataTable.Rows)
          {
            var user = new User()
            {
              UserName = row[0].ToString(),
              Name = row[1].ToString(),
              Email = row[2].ToString(),
              PhoneNumber = row[3].ToString(),
            };
            await _userManager.CreateAsync(user);
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
        }
        catch (Exception ex)
        {
          transaction.Rollback();
          return new ImportUserToDatabaseResponse()
          {
            Success = false,
            Message = "There's some error in the import process. Please check again your file"
        };
      }
      }
      return new ImportUserToDatabaseResponse()
      {
        Success = true,
        Message = "Import success"
      };
    }
  }
}
