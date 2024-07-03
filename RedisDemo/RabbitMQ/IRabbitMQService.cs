
namespace RedisDemo.RabbitMQ
{
    public interface IRabbitMQService
    {
        void CloseConnection();
        void ReceiveMessage(string queueName, Action<string> messageHandler);
        void SendMessage(string queueName, string message);
    }
}