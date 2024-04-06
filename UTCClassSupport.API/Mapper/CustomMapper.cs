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
      CreateMap<BulletinDTO, UploadNewsToBulletinCommand>().ReverseMap();
      CreateMap<BaseRequest, UploadNewsToBulletinCommand>().ReverseMap();
    }
  }

  public class ImportProfile : Profile
  {
    public ImportProfile()
    {
      CreateMap<ImportUserDTO, ImportUserToDatabaseCommand>().ReverseMap();
      CreateMap<ImportTimetableDTO, ImportTimetableCommand>().ReverseMap();
      CreateMap<BaseRequest, ImportTimetableCommand>().ReverseMap();
    }
  }
}
