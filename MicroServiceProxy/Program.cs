using MicroServiceProxy.DatabaseProxy;
using MicroServiceProxy.LoginProxy;
using MicroServiceProxy.MessageProxy;
using MicroServiceProxy.Middleware;
using MicroServiceProxy.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Prometheus;
using Prometheus.HttpMetrics;

var builder = WebApplication.CreateBuilder(args);
const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddSingleton<MetricReporter>();
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ILoginProxyHandler, LoginProxyHandler>();
builder.Services.AddScoped<ILoggerHandler, LoggerHandler>();
builder.Services.AddScoped<IMessageProxyHandler, MessageProxyHandler>();
builder.Services.AddScoped<IDatabaseProxyHandler, DatabaseProxyHandler>();


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy  =>
        {
            policy.WithOrigins().AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});

var app = builder.Build();

// VVVV Prometheus
app.UseMetricServer();
app.UseHttpMetrics();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors(myAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();