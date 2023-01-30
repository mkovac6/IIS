using System;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "IIS",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

var message = "Ovo je poruka pošaljitelja! Pozdrav za IIS!";

var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish("", "IIS", null, body);

Console.WriteLine($"Poruka poslana: {message}");