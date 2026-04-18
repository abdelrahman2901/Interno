using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.RabbitMQ
{
    public class RabbitMQPublisher : IRabbitMQPublisher, IDisposable
    {
        private readonly IConfiguration _Configuration;
        private IChannel _Channel;
        private IConnection _Connection;
        public RabbitMQPublisher(IConfiguration configuration)
        {
            _Configuration = configuration;

        }



        public async Task InitAsync()
        {

            string HostName = _Configuration["RabbitMQ:RabbitMQ_HostName"]!;
            string UserName = _Configuration["RabbitMQ:RabbitMQ_UserName"]!;
            string Password = _Configuration["RabbitMQ:RabbitMQ_Password"]!;
            string Port = _Configuration["RabbitMQ:RabbitMQ_Port"]!;

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


        public async Task Publish<T>(string routingKey, T Message)
        {
            var MessageJson = JsonSerializer.Serialize(Message);
            var MessageBodyBytes = Encoding.UTF8.GetBytes(MessageJson);

            string ExchangeName = "Interno.Microservice";

            await _Channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Direct, durable: true);

            await _Channel.BasicPublishAsync(ExchangeName, routingKey, MessageBodyBytes);
        }

        public void Dispose()
        {
            if (_Channel != null && _Connection != null)
            {
                _Channel.Dispose();
                _Connection?.Dispose();
            }
        }
    }
}
