using Hangfire;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using UTCClassSupport.API;
using UTCClassSupport.API.Common.Mail;
using UTCClassSupport.API.Infrustructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var serverVersion = new MySqlServerVersion(new Version());

builder.Services.AddDbContext<EFContext>(
    dbContextOptions => dbContextOptions
        .UseMySql(builder.Configuration.GetConnectionString("UTCClassSupportDB"), serverVersion)
);

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));


builder.Services.AddInfrustructure();
builder.Services.AddHangfireConfiguration(builder.Configuration);
builder.Services.AddAuthenticatedConfiguration(builder.Configuration);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var cors = "UTC_ClassSupport_CORS";
builder.Services.AddCors(options =>
{
  options.AddPolicy(cors,
                    policy =>
                    {
                      policy.WithOrigins("*")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/V1/swagger.json", "WebAPI");
  });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(cors);
app.UseAuthorization();

app.UseHangfireDashboard();

app.MapControllers();

app.Run();
