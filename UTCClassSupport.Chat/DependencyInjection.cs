using UTCClassSupport.Chat.Models;
using UTCClassSupport.Chat.Repositories;

namespace UTCClassSupport.Chat
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
      services.Configure<ChatDatabaseSettings>(configuration.GetSection("QwertyChatDB"));
      services.AddScoped<IChatRepository, ChatRepository>();
      services.AddScoped<IUserRepository, UserRepository>();
      return services;
    }
  }
}
