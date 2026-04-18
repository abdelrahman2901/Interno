using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CartDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICartServices;
using MediatR;


namespace E_Commerce_Inern_Project.Core.Features.Cart.Query.GetUserCartDetailsQ
{
    public class GetUserCarItemsHandler : IRequestHandler<GetUserCarItemsQuery, Result<CartDetailsResponse>>
    {
        private readonly ICartService _cartService;
        public GetUserCarItemsHandler(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<Result<CartDetailsResponse>> Handle(GetUserCarItemsQuery request, CancellationToken cancellationToken)
        {
            return await _cartService.GetUserCarItemsQuery(request.UserID);
        }
    }
}
