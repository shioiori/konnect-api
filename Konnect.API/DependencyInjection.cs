using Konnect.API.Infrustructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Data.Common;
using System.Text;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Infrustructure.Repositories;
using UTCClassSupport.API.Models;

namespace UTCClassSupport.API
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddInfrustructure(this IServiceCollection services)
    {
      services.AddIdentity<User, Role>(config =>
      {
        config.SignIn.RequireConfirmedEmail = false;
        config.User.RequireUniqueEmail = true;
      })
              .AddEntityFrameworkStores<EFContext>()
              .AddDefaultTokenProviders();

      services.Configure<IdentityOptions>(options =>
      {
        // Thiết lập về Password
        options.Password.RequireDigit = false; // Không bắt phải có số
        options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
        options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
        options.Password.RequireUppercase = false; // Không bắt buộc chữ in
        options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
        options.Password.RequiredUniqueChars = 0; // Số ký tự riêng biệt

        // Cấu hình Lockout - khóa user
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
        options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
        options.Lockout.AllowedForNewUsers = true;

        // Cấu hình về User.
        options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;  // Email là duy nhất

        // Cấu hình đăng nhập.
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
      });

      services.AddMediatR(configuration =>
      {
        configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
      });

      return services;
    }

    public static IServiceCollection AddHangfireConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
      //services.AddHangfire(x =>
      //{
      //  x.UseSqlServerStorage(configuration.GetConnectionString("DBConnection"));
      //});
      //services.AddHangfireServer();
      return services;
    }

    public static IServiceCollection AddAuthenticatedConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddSwaggerGen(options =>
      {
        options.SwaggerDoc("V1", new OpenApiInfo
        {
          Version = "V1",
          Title = "WebAPI",
          Description = "WebAPI"
        });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
          Scheme = "Bearer",
          BearerFormat = "JWT",
          In = ParameterLocation.Header,
          Name = "Authorization",
          Description = "Bearer Authentication with JWT Token",
          Type = SecuritySchemeType.Http
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                }
            },
            new List < string > ()
        }
    });
      });
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  ValidIssuer = configuration["Jwt:Issuer"],
                  ValidAudience = configuration["Jwt:Issuer"],
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
              });
      return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
      services.AddScoped<AccessManager>();
      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<IPostRepository, PostRepository>();
      services.AddScoped<IGroupRepository, GroupRepository>();
      services.AddScoped<NotificationManager>();
      return services;
    }
  }
}
