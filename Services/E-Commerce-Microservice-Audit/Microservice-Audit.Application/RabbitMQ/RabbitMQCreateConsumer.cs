using Microservice_Audit.Application.Features.Audit.Command.CreateAuditCmd;
using Microservice_Audit.Core.ServicesContracts.IAuditsServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Microservice_Audit.Application.RabbitMQ
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
            string RoutingKey = "Interno.Audit";

            string QueueName = "Interno.Audit.Microservice";

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

                CreateAuditRequest? Request = JsonSerializer.Deserialize<CreateAuditRequest>(message);

                    var scope=_ServiceFactory.CreateScope();
                    var AuditService = scope.ServiceProvider.GetRequiredService<IAuditService>();
                    var NotifeResult = await AuditService.CreateAudit(Request);

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
