using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.OrderDTO;
using E_Commerce_Inern_Project.Core.Features.Orders.Command.CreateOrderCmd;
using E_Commerce_Inern_Project.Core.Features.Orders.Command.UpdateOrderCmd;
using E_Commerce_Inern_Project.Core.Features.Orders.Command.UpdateOrderStatusCmd;

namespace E_Commerce_Inern_Project.Core.ServicesContracts.IOrderServices
{
    public interface IOrderService
    {
        public Task<Result<IEnumerable<OrderDetails>>> GetAllOrders();
        public Task<Result<OrderDetails>> GetOrderByID(Guid OrderID);
        public Task<Result<IEnumerable<OrderDetails>>> GetAllUserOrders(Guid CartID);
        public Task<Result<bool>> UpdateOrderStatus(UpdateOrderStatusRequest Order);
        public Task<Result<OrderResponse>> CreateOrder(CreateOrderRequest request);
        public Task<Result<bool>> UpdateOrder(UpdateOrderRequest request);
    }
}
