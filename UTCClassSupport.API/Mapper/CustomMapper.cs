using AutoMapper;
using Microsoft.Office.Interop.Excel;
using UTCClassSupport.API.Application.ImportExcel;
using UTCClassSupport.API.Application.ImportTimetable;
using UTCClassSupport.API.Application.UploadNewsToBulletin;
using UTCClassSupport.API.Authorize.Requests;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests;

namespace UTCClassSupport.API.Mapper
{
  public class CustomMapper
  {
    private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
    {
      var config = new MapperConfiguration(cfg =>
      {
        cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
        cfg.AddProfile<BulletinProfile>();
        cfg.AddProfile<ImportProfile>();
      });

      var mapper = config.CreateMapper();
      return mapper;
    });

    public static IMapper Mapper => Lazy.Value;
  }

  public class UserProfile : Profile
  {
    public UserProfile()
    {
      CreateMap<RegisterRequest, User>().ReverseMap();

    }
  }

  public class BulletinProfile : Profile
  {
    public BulletinProfile()
    {
      CreateMap<BulletinRequest, UploadNewsToBulletinCommand>().ReverseMap();
      CreateMap<UserData, UploadNewsToBulletinCommand>().ReverseMap();

      CreateMap<BulletinRequest, BulletinRequest>().ReverseMap();
    }
  }

  public class ImportProfile : Profile
  {
    public ImportProfile()
    {
      CreateMap<ImportUserRequest, ImportUserToDatabaseCommand>().ReverseMap();
      CreateMap<UserData, ImportUserToDatabaseCommand>().ReverseMap();

      CreateMap<ImportTimetableRequest, ImportTimetableCommand>().ReverseMap();
      CreateMap<UserData, ImportTimetableCommand>().ReverseMap();
    }
  }
}
