using AutoMapper;
using E_Commerce_Inerno.Core.RepositoryContracts.ICartRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CartDTO;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Carts.Query.GetUserCartDetailsQ
{
    public class GetUserCarItemsHandler : IRequestHandler<GetUserCarItemsQuery, Result<CartDetailsResponse>>
    {
        private readonly ICartRepository _cartRepo;
        private readonly IMapper _mapper;

        public GetUserCarItemsHandler(ICartRepository cartRepo, IMapper mapper)
        {
            _cartRepo = cartRepo;
            _mapper = mapper;
        }

        public async Task<Result<CartDetailsResponse>> Handle(GetUserCarItemsQuery request, CancellationToken cancellationToken)
        {
            var result = await _cartRepo.GetUserCarItemsQuery(request.UserID);
            if (result == null)
            {
                return Result<CartDetailsResponse>.NotFound("no Items Was Found For this User");
            }
            return Result<CartDetailsResponse>.Success(_mapper.Map<CartDetailsResponse>(result));
        }
    }
}
