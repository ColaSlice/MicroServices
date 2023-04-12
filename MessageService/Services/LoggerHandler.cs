using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using MessageService.Models;

namespace MessageService.Services
{
    public class LoggerHandler : ILoggerHandler
    {
        private LogMessage _message;
        
        public LoggerHandler()
        {
            _message = new LogMessage();
        }
        
        public void Log(string message)
        {
            _message.Message = message;
            _message.Timestamp = DateTime.Now;
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "service",
                Password = "pass",
                VirtualHost = "/"
            };
            
            var conn = factory.CreateConnection();
            
            using var channel = conn.CreateModel();
            
            channel.QueueDeclare("LoginService", durable: true, exclusive: false);
            
            var jsonString = JsonSerializer.Serialize(_message);
            var body = Encoding.UTF8.GetBytes(jsonString);
            
            channel.BasicPublish("", "LoginService", body: body);
        }
    }
}