using AutoMapper;
using E_Commerce_Inerno.Core.RepositoryContracts.IOrdersRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.OrderDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Orders.Query.GetAllUserOrdersQ
{
    public class GetAllUserOrdersHandler : IRequestHandler<GetAllUserOrdersQuery, Result<IEnumerable<OrderDetails>>>
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IMapper _mapper;

        public GetAllUserOrdersHandler(IOrdersRepository ordersRepository, IMapper mapper)
        {
            _ordersRepository = ordersRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<OrderDetails>>> Handle(GetAllUserOrdersQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Order> orders = await _ordersRepository.GetAllUserOrders(request.UserID);
            if (!orders.Any())
            {
                return Result<IEnumerable<OrderDetails>>.NotFound("Orders Wasnt Found For That User.");
            }

            return Result<IEnumerable<OrderDetails>>.Success(orders.Select(s => _mapper.Map<OrderDetails>(s)));
        }
    }
}
