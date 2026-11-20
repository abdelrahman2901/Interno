using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.OrderDTO;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Orders.Command.CreateOrderCmd
{
    public record CreateOrderRequest(Guid UserID, Guid AddressID, Guid PaymentID, decimal Subtotal
        , Guid ShippingCostID, decimal DiscountAmount, decimal TotalAmount, Guid? OrderCouponID) : IRequest<Result<OrderResponse>>;

}
