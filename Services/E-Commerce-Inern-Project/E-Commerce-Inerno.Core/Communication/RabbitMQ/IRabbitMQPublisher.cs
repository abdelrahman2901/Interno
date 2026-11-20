namespace E_Commerce_Inerno.Core.Communication.RabbitMQ
{
    public interface IRabbitMQPublisher
    {
        Task Publish<T>(string routingKey,T Message);
        Task InitAsync();
    }
}