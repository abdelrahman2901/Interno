using AutoMapper;
using E_Commerce_Inerno.Core.Policies;
using E_Commerce_Inerno.Core.Communication;

using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Polly;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;
using E_Commerce_Inerno.Core.RepositoryContracts.IOrdersRepo;

namespace E_Commerce_Inerno.Core.Features.Orders.Command.UpdateOrderCmd
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderRequest, Result<bool>>
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IMapper _mapper;
        private readonly IInternoEventPublisher _Publisher;
        private readonly IAsyncPolicy _policy;
        

        public UpdateOrderHandler(IOrdersRepository ordersRepository, IPollyPolicy asyncPolicy, IInternoEventPublisher Publisher, IMapper mapper)
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
                await _Publisher.PublishAuditAsync(AuditRequest);
            });

            return Result<bool>.Success(order != null);
        }
    }
}
