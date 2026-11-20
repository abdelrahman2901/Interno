using E_Commerce_Inerno.Core.Domain.RepositoryContracts.IOrdersRepo;
using E_Commerce_Inerno.Core.Policies;
using E_Commerce_Inerno.Core.Communication;

using E_Commerce_Inerno.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Polly;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;
using E_Commerce_Inerno.Core.Communication.DTOs.NotificationsDTO;

namespace E_Commerce_Inerno.Core.Features.Orders.Command.UpdateOrderStatusCmd
{
    public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusRequest, Result<bool>>
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IUserRepository _UserRepo;
        private readonly IInternoEventPublisher _Publisher;
        private readonly IAsyncPolicy _policy;
        
        private readonly string _NotificationRoutingKey = "Interno.Notification";

        public UpdateOrderStatusHandler(IOrdersRepository ordersRepository, IPollyPolicy asyncPolicy, IInternoEventPublisher Publisher, IUserRepository UserRepo)
        {
            _ordersRepository = ordersRepository;
            _UserRepo = UserRepo;
            _Publisher = Publisher;
            _policy = asyncPolicy.GetPolicy();
        }

        public async Task<Result<bool>> Handle(UpdateOrderStatusRequest request, CancellationToken cancellationToken)
        {
            Order? order = await _ordersRepository.GetOrderByID(request.OrderID, true);
            if (order == null)
            {
                return Result<bool>.NotFound("Order Wasnt Found.");
            }
            string JsonOldValues = JsonSerializer.Serialize<Order>(order);

            order.OrderStatus = request.NewStatus;
            if (!await _ordersRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaveChanges.");
            }

            string JsonNewValues = JsonSerializer.Serialize<Order>(order);
            AuditRequest AuditRequest = new(await _UserRepo.GetAdminID(), ActionTypeEnum.Update, nameof(Order), JsonOldValues, JsonNewValues);
            CreateNotificationRequest NotifeRequest = new(order.UserID, $"Order {request.NewStatus} Successfully");
            await _policy.ExecuteAsync(async () =>
            {
                await _Publisher.PublishAuditAsync(AuditRequest);
            });

            await _policy.ExecuteAsync(async () =>
            {
                await _Publisher.PublishNotificationAsync(NotifeRequest);
            });

            return Result<bool>.Success(order != null);
        }
    }
}
