using AutoMapper;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICartItemRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICartRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IOrderItemsRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IOrdersRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IProductRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ITransectionRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IUserRepo;
using E_Commerce_Inern_Project.Core.DTO.OrderDTO;
using E_Commerce_Inern_Project.Core.Features.Orders.Command.CreateOrderCmd;
using E_Commerce_Inern_Project.Core.Features.Orders.Command.UpdateOrderCmd;
using E_Commerce_Inern_Project.Core.Features.Orders.Command.UpdateOrderStatusCmd;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.NotificationsDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IOrderServices;
using Polly;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly ICartRepository _CartRepo;
        private readonly ICartItemRepository _CartItemRepo;
        private readonly IOrderItemsRepoistory _OrderItemRepo;
        private readonly IProductRepository _ProductRepo;
        private readonly ITransectionRepository _TransectionRepo;
        private readonly IUserRepository _UserRepo;
        private readonly IMapper _mapper;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly IAsyncPolicy _asyncPolicy;
        private readonly string _AuditRoutingKey = "Interno.Audit";
        private readonly string _NotificationRoutingKey = "Interno.Notification";

        public OrderService(IOrdersRepository ordersRepository, IAsyncPolicy asyncPolicy, IRabbitMQPublisher Publisher, IUserRepository UserRepo, IProductRepository ProductRepo, ICartRepository CartRepo,
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
            _UserRepo = UserRepo;
            _asyncPolicy = asyncPolicy;
            _Publisher = Publisher;
        }

        public async Task<Result<bool>> UpdateOrderStatus(UpdateOrderStatusRequest request)
        {
            Order? order = await _ordersRepository.GetOrderByID_Tracking(request.OrderID);
            if (order == null)
            {
                return Result<bool>.NotFound("Order Wasnt Found.");
            }
            string JsonOldValues= JsonSerializer.Serialize<Order>(order);

            order.OrderStatus = request.NewStatus;
            if (!await _ordersRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaveChanges.");
            }
            
            string JsonNewValues = JsonSerializer.Serialize<Order>(order);
            AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Update, nameof(Order), JsonOldValues, JsonNewValues  );
            CreateNotificationRequest NotifeRequest = new(order.UserID, $"Order {request.NewStatus} Successfully");
            await _asyncPolicy.ExecuteAsync(async () =>
            {
                await _Publisher.Publish(_AuditRoutingKey, AuditRequest);
            });

            await _asyncPolicy.ExecuteAsync(async () =>
            {
                await _Publisher.Publish(_NotificationRoutingKey, NotifeRequest);
            });

            return Result<bool>.Success(order != null);
        }

        public async Task<Result<OrderResponse>> CreateOrder(CreateOrderRequest request)
        {
            var transection = await _TransectionRepo.BeginTransactionAsync();
            if (transection == null)
            {
                return Result<OrderResponse>.InternalError("Failed To begin Transection");
            }
            try
            {
                Order? order = await _ordersRepository.CreateOrder(_mapper.Map<Order>(request));
                if (order == null)
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

                var orderList = await _ordersRepository.GetOrderByID_NoTracking(order.OrderID);

                foreach (var r in orderList.OrderItems)
                {
                    var product = await _ProductRepo.GetProductByID_Tracking(r.ProductID);
                    product.Stock -= r.Quantity;
                }

                if (!await _ProductRepo.SaveChanges())
                {
                    return Result<OrderResponse>.InternalError("Failed TO Save Product Changes For Stock");
                }

                string JsonNewValues = JsonSerializer.Serialize<Order>(order);
                AuditRequest AuditRequest = new(order.UserID, ActionTypeEnum.Create, nameof(Order), null, JsonNewValues);
                CreateNotificationRequest NotifeRequeset = new(order.UserID, "Order Created Successfully ");
                await _asyncPolicy.ExecuteAsync(async () =>
                {
                    await _Publisher.Publish(_AuditRoutingKey, AuditRequest);
                });

                await _asyncPolicy.ExecuteAsync(async () =>
                {
                    await _Publisher.Publish(_NotificationRoutingKey, AuditRequest);
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
        public async Task<Result<IEnumerable<OrderDetails>>> GetAllOrders()
        {
            IEnumerable<Order> orders = await _ordersRepository.GetAllOrders();
            if (!orders.Any())
            {
                return Result<IEnumerable<OrderDetails>>.NotFound("Orders Wasnt Found.");
            }

            return Result<IEnumerable<OrderDetails>>.Success(orders.Select(s => _mapper.Map<OrderDetails>(s)));
        }

        public async Task<Result<IEnumerable<OrderDetails>>> GetAllUserOrders(Guid UserID)
        {
            IEnumerable<Order> orders = await _ordersRepository.GetAllUserOrders(UserID);
            if (!orders.Any())
            {
                return Result<IEnumerable<OrderDetails>>.NotFound("Orders Wasnt Found For That User.");
            }

            return Result<IEnumerable<OrderDetails>>.Success(orders.Select(s => _mapper.Map<OrderDetails>(s)));
        }

        public async Task<Result<bool>> UpdateOrder(UpdateOrderRequest request)
        {
            Order? order = await _ordersRepository.GetOrderByID_Tracking(request.OrderID);
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
            await _asyncPolicy.ExecuteAsync(async () =>
            {
                await _Publisher.Publish(_AuditRoutingKey, AuditRequest);
            });

            return Result<bool>.Success(order != null);
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
            AuditRequest AuditRequest = new(UserID, ActionTypeEnum.Create, nameof(OrderItems),null,JsonNewValues);
            await _asyncPolicy.ExecuteAsync(async () =>
            {
                await _Publisher.Publish(_AuditRoutingKey, AuditRequest);
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
            await _asyncPolicy.ExecuteAsync(async () =>
            {
                await _Publisher.Publish(_AuditRoutingKey, AuditRequest);
            });
            return true;
        }

        public async Task<Result<OrderDetails>> GetOrderByID(Guid OrderID)
        {
            Order? order = await _ordersRepository.GetOrderByID_NoTracking(OrderID);
            if (order == null)
            {
                return Result<OrderDetails>.NotFound("Order Wasnt Found.");
            }
            return Result<OrderDetails>.Success(_mapper.Map<OrderDetails>(order));
        }

        private async Task<Guid> GetAdminID()
        {
            var admin = await _UserRepo.GetApplicationUserByEmail("admin@gmail.com");
            return admin.Id;
        }
    }
}
