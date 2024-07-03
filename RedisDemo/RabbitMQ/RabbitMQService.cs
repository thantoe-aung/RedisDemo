using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RedisDemo.RabbitMQ
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;


        public RabbitMQService()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost", // RabbitMQ server address
                Port = 5672,            // Default port
                UserName = "guest",     // Default username
                Password = "guest"      // Default password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void CloseConnection()
        {
            _channel.Close();
            _connection.Close();
        }

        public void SendMessage(string queueName, string message)
        {
            
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }

        public void ReceiveMessage(string queueName, Action<string> messageHandler)
        {
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                messageHandler.Invoke(message);
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
    }
}
