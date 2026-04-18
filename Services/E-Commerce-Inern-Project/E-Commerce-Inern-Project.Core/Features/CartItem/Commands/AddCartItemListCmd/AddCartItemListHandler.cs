using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICartItemServices;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.CartItem.Commands.AddCartItemListCmd
{
    public class AddCartItemListHandler : IRequestHandler<AddCartItemListRequest, Result<bool>>
    {
        private readonly ICartItemService _CartitemService;
        public AddCartItemListHandler(ICartItemService cartitemService)
        {
            _CartitemService = cartitemService;
        }

        public async Task<Result<bool>> Handle(AddCartItemListRequest request, CancellationToken cancellationToken)
        {
            return await _CartitemService.AddCartItemList(request.request);

        }
    }
}
