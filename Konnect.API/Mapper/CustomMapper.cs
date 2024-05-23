using AutoMapper;
using Konnect.API.Application.GetCreatedChatData;
using Konnect.API.Data;
using UTCClassSupport.API.Application.AddPostComment;
using UTCClassSupport.API.Application.ClearTimetable;
using UTCClassSupport.API.Application.DeleteTimetable;
using UTCClassSupport.API.Application.GetGroupByUser;
using UTCClassSupport.API.Application.GetNews;
using UTCClassSupport.API.Application.GetNotification;
using UTCClassSupport.API.Application.GetUserTimetable;
using UTCClassSupport.API.Application.ImportExcel;
using UTCClassSupport.API.Application.ImportTimetable;
using UTCClassSupport.API.Application.InviteToGroup;
using UTCClassSupport.API.Application.OutGroup;
using UTCClassSupport.API.Application.ScheduleTimetableRemind;
using UTCClassSupport.API.Application.UpdateStateNotification;
using UTCClassSupport.API.Application.UpdateTimetable;
using UTCClassSupport.API.Application.UploadNewsToBulletin;
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
        cfg.AddProfile<NotificationProfile>();
        cfg.AddProfile<ChatProfile>();
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
      CreateMap<UserDTO, User>().ReverseMap();
      CreateMap<AddUserRequest, User>().ReverseMap();
      CreateMap<UpdateUserRequest, User>().ReverseMap();
      CreateMap<UserData, User>().ReverseMap();

    }
  }

  public class GroupProfile : Profile
  {
    public GroupProfile()
    {
      CreateMap<Group, GroupDTO>().ReverseMap();
      CreateMap<UserInfo, GetGroupByUserQuery>().ReverseMap();

      CreateMap<UserInfo, InviteToGroupCommand>().ReverseMap();
      CreateMap<InviteToGroupRequest, InviteToGroupCommand>().ReverseMap();

      CreateMap<UserInfo, OutGroupCommand>().ReverseMap();
      CreateMap<GroupData, Group>().ReverseMap();
    }
  }

  public class BulletinProfile : Profile
  {
    public BulletinProfile()
    {
      CreateMap<BulletinRequest, UploadNewsToBulletinCommand>().ReverseMap();
      CreateMap<UserInfo, UploadNewsToBulletinCommand>().ReverseMap();
      CreateMap<UserInfo, AddPostCommentCommand>().ReverseMap();
      CreateMap<UserInfo, GetPostQuery>().ReverseMap();
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
      CreateMap<UserInfo, ImportUserToDatabaseCommand>().ReverseMap();

      CreateMap<ImportTimetableRequest, ImportTimetableCommand>().ReverseMap();
      CreateMap<UserInfo, ImportTimetableCommand>().ReverseMap();
    }
  }

  public class TimetableProfile : Profile
  {
    public TimetableProfile()
    {
      CreateMap<EventDTO, Event>().ReverseMap();
      CreateMap<UserInfo, GetUserTimetableQuery>().ReverseMap();
      CreateMap<UserInfo, DeleteTimetableCommand>().ReverseMap();
      CreateMap<UserInfo, SynchronizeTimetableWithGoogleCalendarCommand>().ReverseMap();
      CreateMap<UserInfo, UpdateRemindTimetableCommand>().ReverseMap();
      CreateMap<UserInfo, AddEventCommand>().ReverseMap();
      CreateMap<EventRequest, AddEventCommand>().ReverseMap();
    }
  }

  public class NotificationProfile : Profile
  {
    public NotificationProfile()
    {
      CreateMap<UserInfo, GetNotificationQuery>().ReverseMap();
      CreateMap<UserInfo, UpdateStateNotificationCommand>().ReverseMap();
    }
  }

  public class ChatProfile : Profile
  {
    public ChatProfile()
    {
      CreateMap<UserInfo, GetCreateChatDataQuery>().ReverseMap();
    }
  }
}
