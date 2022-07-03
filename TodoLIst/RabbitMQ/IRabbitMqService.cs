using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace TodoLIst.RabbitMQ
{
    public interface IRabbitMqService
    {
        void SendMessage(object obj, string comment);
        void SendMessage(string message, string comment);
    }

    public class RabbitMqService : IRabbitMqService
    { 
        public void SendMessage(object obj, string comment)
        {
            var message = JsonSerializer.Serialize(obj);
            SendMessage(message, comment);
        }

        public void SendMessage(string message, string comment)
        {
            // По хорошему значения хоста, обменника и очереди нужно забирать из конфига, однако в таком тестовом проекте решил не заморачиваться
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "dev-queue",
                               durable: true, //по умолчанию здесь false, true стоит т.к. в моей очереди стоит true
                               exclusive: false,
                               autoDelete: false,
                               arguments: null);

                var body = Encoding.UTF8.GetBytes($"{comment}: {message}");

                channel.BasicPublish(exchange: "",
                               routingKey: "dev-queue",
                               basicProperties: null,
                               body: body);
            }
        }
    }



}
