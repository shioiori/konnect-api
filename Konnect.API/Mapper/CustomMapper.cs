using AutoMapper;
using Konnect.API.Application.AddGroup;
using Konnect.API.Application.DeleteGroup;
using Konnect.API.Application.EditGroup;
using Konnect.API.Application.GetCreatedChatData;
using Konnect.API.Application.GetGroup;
using Konnect.API.Application.GetPost;
using Konnect.API.Application.KickFromGroup;
using Konnect.API.Data;
using UTCClassSupport.API.Application.AddPostComment;
using UTCClassSupport.API.Application.ChangeNewsState;
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
			CreateMap<RoleDTO, Role>().ReverseMap();

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

			//CreateMap<UserInfo, AddGroupCommand>().ReverseMap();
			CreateMap<AddGroupRequest, Group>().ReverseMap();

			CreateMap<UserInfo, DeleteGroupCommand>().ReverseMap();
			CreateMap<UserInfo, GetGroupQuery>().ReverseMap();
			CreateMap<UserInfo, EditGroupCommand>().ReverseMap();
			CreateMap<UserInfo, KickFromGroupCommand>().ReverseMap();
		}
	}

	public class BulletinProfile : Profile
	{
		public BulletinProfile()
		{
			CreateMap<BulletinRequest, UploadNewsToBulletinCommand>().ReverseMap();
			CreateMap<UserInfo, UploadNewsToBulletinCommand>().ReverseMap();
			CreateMap<UserInfo, AddPostCommentCommand>().ReverseMap();
			CreateMap<UserInfo, GetPostsQuery>().ReverseMap();
			CreateMap<UserInfo, GetPostQuery>().ReverseMap();
			CreateMap<CommentRequest, AddPostCommentCommand>().ReverseMap();
			CreateMap<ChangePostStateRequest, ChangePostStateCommand>().ReverseMap();
			CreateMap<UserInfo, ChangePostStateCommand>().ReverseMap();

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
			CreateMap<EventDTO, Event>()
				.ForMember(x => x.From, y => y.MapFrom(x => DateTime.Parse(x.From)))
				.ForMember(x => x.To, y => y.MapFrom(x => DateTime.Parse(x.To)));
			CreateMap<UserInfo, GetUserTimetableQuery>().ReverseMap();
			CreateMap<UserInfo, DeleteTimetableCommand>().ReverseMap();
			CreateMap<UserInfo, SynchronizeTimetableWithGoogleCalendarCommand>().ReverseMap();
			CreateMap<UserInfo, UpdateRemindTimetableCommand>().ReverseMap();
			CreateMap<UserInfo, AddEventCommand>().ReverseMap();
			CreateMap<AddEventRequest, AddEventCommand>()
				.ForMember(x => x.Start, y => y.MapFrom(x => DateTime.Parse(x.Start)))
				.ForMember(x => x.End, y => y.MapFrom(x => DateTime.Parse(x.End)));
			CreateMap<UpdateEventRequest, Event>()
				.ForMember(x => x.From, y => y.MapFrom(x => DateTime.Parse(x.From)))
				.ForMember(x => x.To, y => y.MapFrom(x => DateTime.Parse(x.To)));
			CreateMap<Event, EventDTO>()
				.ForMember(x => x.From, y => y.MapFrom(x => x.From.ToString("yyyy-MM-dd HH:mm:ss")))
				.ForMember(x => x.To, y => y.MapFrom(x => x.To.ToString("yyyy-MM-dd HH:mm:ss")));
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
