using System.Text;
using System.Text.Json;
using LoggerMachine.Models;
using LoggerMachine.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

DatabaseHandler databaseHandler = new DatabaseHandler();

Console.WriteLine("Hello, World!");

/*
List<Log> temp = databaseHandler.Read();
for (int i = 0; i < temp.Count; i++)
{
    Console.WriteLine(temp[i].Message + " " + temp[i].Timestamp);
} 
*/

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

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Log log = JsonSerializer.Deserialize<Log>(message);
    databaseHandler.Insert(log.Message, log.Timestamp);
    Console.WriteLine($"Recieved message: {message}");
};

channel.BasicConsume("LoginService", true, consumer);

Console.ReadLine();



