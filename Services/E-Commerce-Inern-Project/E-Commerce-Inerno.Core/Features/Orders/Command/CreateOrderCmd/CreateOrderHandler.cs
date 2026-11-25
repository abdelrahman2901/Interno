using AutoMapper;
using E_Commerce_Inerno.Core.Domain.RepositoryContracts.ITransectionRepo;
using E_Commerce_Inerno.Core.Policies;
using E_Commerce_Inerno.Core.Communication;

using E_Commerce_Inerno.Core.RepositoryContracts.ICartItemRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.ICartRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IOrderItemsRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IProductRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.OrderDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Polly;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;
using E_Commerce_Inerno.Core.Communication.DTOs.NotificationsDTO;
using E_Commerce_Inerno.Core.RepositoryContracts.IOrdersRepo;

namespace E_Commerce_Inerno.Core.Features.Orders.Command.CreateOrderCmd
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, Result<OrderResponse>>
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly ICartRepository _CartRepo;
        private readonly ICartItemRepository _CartItemRepo;
        private readonly IOrderItemsRepoistory _OrderItemRepo;
        private readonly IProductRepository _ProductRepo;
        private readonly ITransectionRepository _TransectionRepo;
        private readonly IMapper _mapper;
        private readonly IInternoEventPublisher _Publisher;
        private readonly IAsyncPolicy _policy;
        
        private readonly string _NotificationRoutingKey = "Interno.Notification";

        public CreateOrderHandler(IOrdersRepository ordersRepository, IPollyPolicy asyncPolicy, IInternoEventPublisher Publisher, IProductRepository ProductRepo, ICartRepository CartRepo,
            IOrderItemsRepoistory OrderItemRepo, ITransectionRepository TransectionRepo,
            ICartItemRepository CartItemRepo, IMapper mapper)
        {
            _ordersRepository = ordersRepository;
            _CartRepo = CartRepo;
            _CartItemRepo = CartItemRepo;
            _TransectionRepo = TransectionRepo;
            _ProductRepo = ProductRepo;
            _OrderItemRepo = OrderItemRepo;
            _mapper = mapper;
            _Publisher = Publisher;
            _policy = asyncPolicy.GetPolicy();
        }

        public async Task<Result<OrderResponse>> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var transection = await _TransectionRepo.BeginTransactionAsync();
            if (transection == null)
            {
                return Result<OrderResponse>.InternalError("Failed To begin Transection");
            }
            try
            {
                Order order = _mapper.Map<Order>(request);
                bool result = await _ordersRepository.CreateOrder(order);
                if (!result)
                {
                    return Result<OrderResponse>.InternalError("Failed To Add Order.");
                }
                if (!await _ordersRepository.SaveChanges())
                {
                    return Result<OrderResponse>.InternalError("Failed To SaveChanges. for orders");
                }
                var itemREsult = await AddOrdeItems(order.OrderID, order.UserID);

                if (!itemREsult.IsSuccess)
                {
                    await transection.RollbackAsync();
                    return Result<OrderResponse>.InternalError(itemREsult.ErrorMessage);
                }

                var orderList = await _ordersRepository.GetOrderByID(order.OrderID, false);

                foreach (var r in orderList.OrderItems)
                {
                    var product = await _ProductRepo.GetProductByID(r.ProductID, true);
                    product.Stock -= r.Quantity;
                }

                if (!await _ProductRepo.SaveChanges())
                {
                    return Result<OrderResponse>.InternalError("Failed TO Save Product Changes For Stock");
                }

                string JsonNewValues = JsonSerializer.Serialize<Order>(order);
                AuditRequest AuditRequest = new(order.UserID, ActionTypeEnum.Create, nameof(Order), null, JsonNewValues);
                CreateNotificationRequest NotifeRequeset = new(order.UserID, "Order Created Successfully ");
                await _policy.ExecuteAsync(async () =>
                {
                    await _Publisher.PublishAuditAsync(AuditRequest);
                });

                await _policy.ExecuteAsync(async () =>
                {
                    await _Publisher.PublishNotificationAsync(NotifeRequeset);
                });

                await transection.CommitAsync();
                return Result<OrderResponse>.Success(_mapper.Map<OrderResponse>(order));
            }
            catch (Exception ex)
            {
                await transection.RollbackAsync();
                return Result<OrderResponse>.InternalError("Something Went Wrong Check Logs");
            }
        }

        private async Task<Result<bool>> AddOrdeItems(Guid orderID, Guid UserID)
        {
            Cart? cart = await _CartRepo.GetCartByUserID(UserID);

            if (cart == null || !cart.CartItems.Any())
            {
                return Result<bool>.NotFound("No Cart Items Was Found");
            }

            IEnumerable<CartItems> cartItems = await _CartItemRepo.GetCartItemsByCaerID(cart.CartID);

            var orderItems = cartItems.Select(item => _mapper.Map<OrderItems>(item)).ToList();
            foreach (OrderItems item in orderItems)
            {
                item.OrderID = orderID;
            }

            bool orderItemInsertResult = await _OrderItemRepo.AddOrderItemList(orderItems);
            if (!orderItemInsertResult)
            {
                return Result<bool>.InternalError("Failed To Add Order Items");
            }

            string JsonNewValues = JsonSerializer.Serialize<List<OrderItems>>(orderItems);
            AuditRequest AuditRequest = new(UserID, ActionTypeEnum.Create, nameof(OrderItems), null, JsonNewValues);

            await _policy.ExecuteAsync(async () =>
            {
                await _Publisher.PublishAuditAsync(AuditRequest);
            });

            bool CartItemsDeletionResult = await DeleteCartItems(cartItems, UserID);

            return Result<bool>.Success(orderItemInsertResult);
        }

        private async Task<bool> DeleteCartItems(IEnumerable<CartItems> cartItems, Guid UserID)
        {
            string JsonOldValues = JsonSerializer.Serialize<IEnumerable<CartItems>>(cartItems);
            foreach (var item in cartItems)
            {
                item.IsDeleted = true;
            }
            if (!await _CartItemRepo.SaveChanges())
            {
                return false;
            }
            string JsonNewValues = JsonSerializer.Serialize<IEnumerable<CartItems>>(cartItems);
            AuditRequest AuditRequest = new(UserID, ActionTypeEnum.Delete, nameof(CartItems), JsonOldValues, JsonNewValues);

            await _policy.ExecuteAsync(async () =>
            {
                await _Publisher.PublishAuditAsync(AuditRequest);
            });
            return true;
        }
    }
}
