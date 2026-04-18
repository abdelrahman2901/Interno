using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IOrderServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Orders.Command.UpdateOrderStatusCmd
{
    public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusRequest, Result<bool>>
    {
        private readonly IOrderService _orderService;
        public UpdateOrderStatusHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<Result<bool>> Handle(UpdateOrderStatusRequest request, CancellationToken cancellationToken)
        {
            return await _orderService.UpdateOrderStatus(request);
        }
    }
}
