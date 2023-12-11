using LoggingService.Application;
using LoggingService.DataAccess.Postgres;
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

var app = builder.Build();
app.UseHttpLogging();
app.UseCors(cfg =>
{
    cfg.AllowAnyOrigin();
    cfg.AllowAnyHeader();
    cfg.AllowAnyMethod();
});
app.MapControllers();

app.Run();
