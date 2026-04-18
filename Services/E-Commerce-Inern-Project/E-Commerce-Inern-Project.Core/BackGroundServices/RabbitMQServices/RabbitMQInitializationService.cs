using E_Commerce_Inern_Project.Core.RabbitMQ;
using Microsoft.Extensions.Hosting;

namespace E_Commerce_Inern_Project.Core.BackGroundServices.RabbitMQServices
{
    public class RabbitMQInitializationService : BackgroundService
    {

        private readonly IRabbitMQPublisher _Publisher;
        public RabbitMQInitializationService(IRabbitMQPublisher Publisher)
        {
            _Publisher = Publisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _Publisher.InitAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
