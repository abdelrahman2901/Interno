using AutoMapper;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IOrdersRepo;
using E_Commerce_Inern_Project.Core.Policies;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Polly;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.Orders.Command.UpdateOrderCmd
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderRequest, Result<bool>>
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IMapper _mapper;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly IAsyncPolicy _policy;
        private readonly string _AuditRoutingKey = "Interno.Audit";

        public UpdateOrderHandler(IOrdersRepository ordersRepository, IPollyPolicy asyncPolicy, IRabbitMQPublisher Publisher, IMapper mapper)
        {
            _ordersRepository = ordersRepository;
            _mapper = mapper;
            _Publisher = Publisher;
            _policy = asyncPolicy.GetPolicy();
        }

        public async Task<Result<bool>> Handle(UpdateOrderRequest request, CancellationToken cancellationToken)
        {
            Order? order = await _ordersRepository.GetOrderByID(request.OrderID, true);
            if (order == null)
            {
                return Result<bool>.NotFound("Order Wasnt Found.");
            }
            string JsonOldValues = JsonSerializer.Serialize<Order>(order);
            _mapper.Map(request, order);

            if (!await _ordersRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaveChanges.");
            }

            string JsonNewValues = JsonSerializer.Serialize<Order>(order);
            AuditRequest AuditRequest = new(order.UserID, ActionTypeEnum.Update, nameof(Order), JsonOldValues, JsonNewValues);

            await _policy.ExecuteAsync(async () =>
            {
                await _Publisher.Publish(_AuditRoutingKey, AuditRequest);
            });

            return Result<bool>.Success(order != null);
        }
    }
}
