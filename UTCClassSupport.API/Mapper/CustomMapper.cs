using AutoMapper;
using Microsoft.Office.Interop.Excel;
using UTCClassSupport.API.Application.ImportExcel;
using UTCClassSupport.API.Application.UploadNewsToBulletin;
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
      });

      var mapper = config.CreateMapper();
      return mapper;
    });

    public static IMapper Mapper => Lazy.Value;
  }

  public class BulletinProfile : Profile
  {
    public BulletinProfile()
    {
      CreateMap<BulletinDTO, UploadNewsToBulletinCommand>().ReverseMap();
    }
  }

  public class ImportProfile : Profile
  {
    public ImportProfile()
    {
      CreateMap<ImportUserDTO, ImportUserToDatabaseCommand>().ReverseMap();
    }
  }
}
