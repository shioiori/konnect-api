using Konnect.ChatHub.Models.Database;
using Konnect.ChatHub.Repositories;

namespace Konnect.ChatHub
{
    public static class DependencyInjection
  {
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
      services.Configure<ChatDatabaseSettings>(configuration.GetSection("QwertyChatDB"));
      services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
      services.AddScoped<IChatRepository, ChatRepository>();
      services.AddScoped<IUserRepository, IUserRepository>();
      services.AddScoped<IMessageRepository, IMessageRepository>();
      services.AddScoped<IUnitOfWork, UnitOfWork>();
      return services;
    }
  }
}
