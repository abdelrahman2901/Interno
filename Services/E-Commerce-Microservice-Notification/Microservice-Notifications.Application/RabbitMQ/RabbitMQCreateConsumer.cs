using Microservice_Notifications.Application.Features.Notification.Command.CreateNotifcationCmd;
using Microservice_Notifications.Application.ServicesContracts.INotificationsServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Microservice_Notifications.Application.RabbitMQ
{
    public class RabbitMQCreateConsumer : IDisposable, IRabbitMQCreateConsumer
    {
        private readonly IConfiguration _Configuration;
        private readonly IServiceScopeFactory _ServiceFactory;
        private IConnection _Connection;
        private IChannel _Channel;

        public RabbitMQCreateConsumer(IConfiguration configuration, IServiceScopeFactory ServiceFactory)
        {
            _Configuration = configuration;
            _ServiceFactory = ServiceFactory;
        }
        public async Task InitAsync()
        {

            string HostName = _Configuration["RabbitMQ_HostName"]!;
            string UserName = _Configuration["RabbitMQ_UserName"]!;
            string Password = _Configuration["RabbitMQ_Password"]!;
            string Port = _Configuration["RabbitMQ_Port"]!;

            ConnectionFactory Factory = new()
            {
                HostName = HostName,
                Port = Convert.ToInt32(Port),
                UserName = UserName,
                Password = Password
            };

            _Connection = await Factory.CreateConnectionAsync();
            _Channel = await _Connection.CreateChannelAsync();
        }
        public async Task Consume()
        {
            string RoutingKey = "Interno.Notification";

            string QueueName = "Interno.Notification.Microservice";

            string ExchangeName = "Interno.Microservice";
            await _Channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Direct, durable: true);
            await _Channel.QueueDeclareAsync(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            await _Channel.QueueBindAsync(queue: QueueName, routingKey: RoutingKey, exchange: ExchangeName);

            AsyncEventingBasicConsumer Consumer = new(_Channel);

            Consumer.ReceivedAsync += async (sender, Args) =>
            {
                byte[] body=Args.Body.ToArray();
                string? message = Encoding.UTF8.GetString(body);

                if (message != null)
                {

                CreateNotificationRequest? Request = JsonSerializer.Deserialize<CreateNotificationRequest>(message);

                    var scope=_ServiceFactory.CreateScope();
                    var NotificationService = scope.ServiceProvider.GetRequiredService<INotificationsService>();
                    var NotifeResult = await NotificationService.CreateNotification(Request);

                }
            };

           await _Channel.BasicConsumeAsync(queue: QueueName, autoAck: true,consumer: Consumer);
        }

        public void Dispose()
        {
            _Channel.Dispose();
            _Connection.Dispose();
        }
    }
}
