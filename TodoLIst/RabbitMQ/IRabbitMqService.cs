using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace TodoLIst.RabbitMQ
{
    public interface IRabbitMqService
    {
        void SendMessage(object obj);
        void SendMessage(string message);
    }

    public class RabbitMqService : IRabbitMqService
    {
        public void SendMessage(object obj)
        {
            var message = JsonSerializer.Serialize(obj);
            SendMessage(message);
        }

        public void SendMessage(string message)
        {
            // По хорошему значения "localhost" и "MyQueue" нужно вынести в конфиг
            var factory = new ConnectionFactory()
            {
                HostName = "host_name",
                UserName = "user_name",
                Password = "p@ssword",
                AutomaticRecoveryEnabled = true
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "MyQueue",
                               durable: false,
                               exclusive: false,
                               autoDelete: false,
                               arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                               routingKey: "MyQueue",
                               basicProperties: null,
                               body: body);
            }
        }
    }



}
