using AutoMapper;
using Konnect.ChatHub.Data;
using Konnect.ChatHub.Models;
using Konnect.ChatHub.Responses;

namespace Konnect.ChatHub.Mapper
{
  public class CustomMapper
  {
    private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
    {
      var config = new MapperConfiguration(cfg =>
      {
        cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
        cfg.AddProfile<ChatProfile>();
      });

      var mapper = config.CreateMapper();
      return mapper;
    });

    public static IMapper Mapper => Lazy.Value;
  }

  public class ChatProfile : Profile
  {
    public ChatProfile()
    {
      CreateMap<Chat, ChatResponse>().ReverseMap();
      CreateMap<Message, MessageResponse>().ReverseMap();
      CreateMap<User, UserResponse>().ReverseMap();
      CreateMap<Group, GroupResponse>().ReverseMap();
      CreateMap<User, UserData>().ReverseMap();
      CreateMap<Group, GroupData>().ReverseMap();
    }
  }
}
