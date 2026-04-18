using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IOrderServices;
using MediatR;
 
namespace E_Commerce_Inern_Project.Core.Features.Orders.Command.UpdateOrderCmd
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderRequest, Result<bool>>
    {
        private readonly IOrderService _orderService;
        public UpdateOrderHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<Result<bool>> Handle(UpdateOrderRequest request, CancellationToken cancellationToken)
        {
            return await _orderService.UpdateOrder(request);
        }
    }
}