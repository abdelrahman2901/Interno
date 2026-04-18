namespace Microservice_Notifications.Application.RabbitMQ
{
    public interface IRabbitMQCreateConsumer
    {
        Task Consume();
        Task InitAsync();
    }
}