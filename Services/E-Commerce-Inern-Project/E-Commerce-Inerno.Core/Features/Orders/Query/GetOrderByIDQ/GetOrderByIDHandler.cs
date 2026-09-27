using AutoMapper;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IOrdersRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.OrderDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Orders.Query.GetOrderByIDQ
{
    public class GetOrderByIDHandler : IRequestHandler<GetOrderByIDQuery, Result<OrderDetails>>
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IMapper _mapper;

        public GetOrderByIDHandler(IOrdersRepository ordersRepository, IMapper mapper)
        {
            _ordersRepository = ordersRepository;
            _mapper = mapper;
        }

        public async Task<Result<OrderDetails>> Handle(GetOrderByIDQuery request, CancellationToken cancellationToken)
        {
            Order? order = await _ordersRepository.GetOrderByID(request.OrderID, false);
            if (order == null)
            {
                return Result<OrderDetails>.NotFound("Order Wasnt Found.");
            }
            return Result<OrderDetails>.Success(_mapper.Map<OrderDetails>(order));
        }
    }
}
