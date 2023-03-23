using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using LoginService.Models;

namespace LoginService.Services;
public class LoggerHandler : ILoggerHandler
{
    public LoggerHandler()
    {
        
    }

    public void Log(LogMessage message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "user",
            Password = "pass",
            VirtualHost = "/"
        };
        
        var conn = factory.CreateConnection();
        
        using var channel = conn.CreateModel();
        
        channel.QueueDeclare("LoginService", durable: true, exclusive: false);
        
        var jsonString = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonString);
        
        channel.BasicPublish("", "LoginService", body: body);
    }
}