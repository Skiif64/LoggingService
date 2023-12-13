using LoggingService.Application;
using LoggingService.DataAccess.Ef;
using LoggingService.WebApi.Hubs;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpLogging(cfg =>
{
    cfg.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponseStatusCode;
});

builder.Services.AddCors();
builder.Services.AddApplication();
builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddControllers();
var mapsterConfig = new TypeAdapterConfig();
mapsterConfig.Scan(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton(mapsterConfig);
builder.Services.AddScoped<IMapper, ServiceMapper>();

builder.Services.AddSignalR();
builder.Services.AddMediatR(cfg => //TODO: add only notificationHandlers
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
});

var app = builder.Build();
app.UseHttpLogging();
app.UseCors(cfg =>
{
    cfg.AllowAnyOrigin();
    cfg.AllowAnyHeader();
    cfg.AllowAnyMethod();
});
app.MapControllers();
app.MapHub<NotificationHub>("hub/notification");

app.Run();
