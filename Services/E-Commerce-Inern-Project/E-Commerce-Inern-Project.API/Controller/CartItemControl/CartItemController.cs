using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CartItemDTO;
using E_Commerce_Inern_Project.Core.Features.CartItem.Commands.AddCartItemListCmd;
using E_Commerce_Inern_Project.Core.Features.CartItem.Commands.CreateCartItemCmd;
using E_Commerce_Inern_Project.Core.Features.CartItem.Commands.DeleteCartItemCmd;
using E_Commerce_Inern_Project.Core.Features.CartItem.Commands.UpdateCartItemCmd;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controller.CartItemControl
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartItemController : ControllerBase
    {
        private readonly IMediator _MediatR;
        public CartItemController(IMediator mediatR)
        {
            _MediatR = mediatR;
        }

        [HttpPost("AddCartItem")] 
        public async Task<Result<bool>> AddCartItem(CreateCartItemRequest request)
        {
            return await _MediatR.Send(request);
        }
        [HttpPost("AddCartItemList")] 
        public async Task<Result<bool>> AddCartItemList(IEnumerable<CreateCartItemRequest >request)
        {
            return await _MediatR.Send(new AddCartItemListRequest(request));
        }
        [HttpPut("UpdateCartItem")]
        public async Task<Result<CartItemResponse>> UpdateCartItem(UpdateCartItemRequest request)
        {
            return await _MediatR.Send(request);
        }
        [HttpPut("DeleteCartItem/{CartItemID}")]
        public async Task<Result<bool>> DeleteCartItem(Guid CartItemID)
        {
            return await _MediatR.Send(new DeleteCartItemRequest(CartItemID));
        }
    }
}
