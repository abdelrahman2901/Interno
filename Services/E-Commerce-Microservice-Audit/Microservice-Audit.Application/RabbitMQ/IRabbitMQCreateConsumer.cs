namespace Microservice_Audit.Application.RabbitMQ
{
    public interface IRabbitMQCreateConsumer
    {
        Task Consume();
        Task InitAsync();
    }
}