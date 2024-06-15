using Konnect.ChatHub;
using Konnect.ChatHub.Hubs;
using Konnect.ChatHub.Models.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddServices(builder.Configuration);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<ChatDatabaseSettings>(builder.Configuration.GetSection("ChatDB"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var cors = "UTC_ClassSupport_CORS";
builder.Services.AddCors(options =>
{
	options.AddPolicy(cors,
				  policy =>
				  {
					  policy.WithOrigins("http://localhost:5173").WithOrigins("http://192.168.1.3:5173").WithOrigins("http://konnect.com:5173")
							  .AllowAnyHeader()
							  .AllowAnyMethod()
									  .AllowCredentials(); ;
				  });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors(cors);

app.MapControllers();
app.MapHub<ChatHub>("/chat-hub");
app.Run();
