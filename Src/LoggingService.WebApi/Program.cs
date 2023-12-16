using LoggingService.Application;
using LoggingService.DataAccess.Ef;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.HttpLogging;
using LoggingService.Endpoints.Application;
using LoggingService.Endpoints.Frontend;
using LoggingService.Endpoints.SignalR;
using LoggingService.Application.Auth;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpLogging(cfg =>
{
    cfg.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponseStatusCode;
});

builder.Services.AddCors();
builder.Services.AddApplication();
builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddApplicationAuthentication();
builder.Services.AddControllers();
var mapsterConfig = new TypeAdapterConfig();
mapsterConfig.Scan(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton(mapsterConfig);
builder.Services.AddScoped<IMapper, ServiceMapper>();
builder.Services.AddHubs();

var app = builder.Build();
app.UseHttpLogging();
app.UseCors(cfg =>
{
    cfg.AllowAnyOrigin();
    cfg.AllowAnyHeader();
    cfg.AllowAnyMethod();
});
app.MapControllers();

app.UseApplicationAuthenticationEndpoints();

app.UseApplicationEndpoints();
app.UseFrontendEndpoints();
app.UseHubs();

app.Run();
