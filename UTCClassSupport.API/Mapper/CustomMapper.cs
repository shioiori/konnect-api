using AutoMapper;
using UTCClassSupport.API.Application.AddPostComment;
using UTCClassSupport.API.Application.ClearTimetable;
using UTCClassSupport.API.Application.DeleteTimetable;
using UTCClassSupport.API.Application.GetGroupByUser;
using UTCClassSupport.API.Application.GetUserTimetable;
using UTCClassSupport.API.Application.ImportExcel;
using UTCClassSupport.API.Application.ImportTimetable;
using UTCClassSupport.API.Application.UpdateTimetable;
using UTCClassSupport.API.Application.UploadNewsToBulletin;
using UTCClassSupport.API.Authorize.Requests;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses.DTOs;

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
        cfg.AddProfile<UserProfile>();
        cfg.AddProfile<GroupProfile>();
        cfg.AddProfile<TimetableProfile>();
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

  public class GroupProfile : Profile
  {
    public GroupProfile()
    {
      CreateMap<Group, GroupDTO>().ReverseMap();
      CreateMap<UserData, GetGroupByUserQuery>().ReverseMap();
    }
  }

  public class BulletinProfile : Profile
  {
    public BulletinProfile()
    {
      CreateMap<BulletinRequest, UploadNewsToBulletinCommand>().ReverseMap();
      CreateMap<UserData, UploadNewsToBulletinCommand>().ReverseMap();
      CreateMap<UserData, AddPostCommentCommand>().ReverseMap();
      CreateMap<CommentRequest, AddPostCommentCommand>().ReverseMap();

      CreateMap<Comment, CommentDTO>().ReverseMap();
      CreateMap<Bulletin, PostDTO>().ReverseMap();
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

  public class TimetableProfile : Profile
  {
    public TimetableProfile()
    {
      CreateMap<ShiftDTO, Shift>().ReverseMap();
      CreateMap<UserData, GetUserTimetableQuery>().ReverseMap();
      CreateMap<UserData, DeleteTimetableCommand>().ReverseMap();
      CreateMap<UserData, SynchronizeTimetableWithGoogleCalendarCommand>().ReverseMap();
      CreateMap<UserData, UpdateRemindTimetableCommand>().ReverseMap();
    }
  }
}
