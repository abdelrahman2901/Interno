namespace E_Commerce_Inern_Project.Core.RabbitMQ
{
    public interface IRabbitMQPublisher
    {
        Task Publish<T>(string routingKey,T Message);
        Task InitAsync();

    }
}
