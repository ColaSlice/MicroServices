using LoginService.Database;
using LoginService.Login;
using LoginService.Middleware;
using LoginService.Services;
using Microsoft.ApplicationInsights.Extensibility;

var builder = WebApplication.CreateBuilder(args);
new TelemetryConfiguration().DisableTelemetry = true;
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inject a LoginHandler
builder.Services.AddScoped<ILoginDatabaseHandler, LoginDatabaseHandler>();
builder.Services.AddScoped<ILoginHandler, LoginHandler>();
builder.Services.AddScoped<ILoggerHandler, LoggerHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();