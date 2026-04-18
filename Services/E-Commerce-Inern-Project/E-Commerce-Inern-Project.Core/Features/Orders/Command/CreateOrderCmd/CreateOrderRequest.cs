using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.OrderDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Orders.Command.CreateOrderCmd
{
    public record CreateOrderRequest( Guid UserID, Guid AddressID, Guid PaymentID, decimal Subtotal
        , Guid ShippingCostID, decimal DiscountAmount, decimal TotalAmount, Guid? OrderCouponID) : IRequest<Result<OrderResponse>>;

}
