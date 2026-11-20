using E_Commerce_Inerno.Core.Features.CartItem.Commands.AddCartItemListCmd;
using E_Commerce_Inerno.Core.Features.CartItem.Commands.DeleteCartItemCmd;
using E_Commerce_Inerno.Core.Features.CartItem.Commands.UpdateCartItemCmd;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CartItemDTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce_Inerno.API.Controllers.CartItemControl.Commands
{
    [Route("api/CartItem")]
    [ApiController]
    [Authorize]
    public class CartItemCommandsController : ControllerBase
    {
        private readonly IMediator _MediatR;

        public CartItemCommandsController(IMediator mediatR)
        {
            _MediatR = mediatR;
        }

        [HttpPost("[action]")]
        public async Task<Result<bool>> AddCartItemList(AddCartItemListRequest request)
        {
            var UserStrID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(UserStrID))
            {
                return Result<bool>.UnAuthorized("User not Authorized");
            }

            request.UserID = Guid.Parse(UserStrID);
            return await _MediatR.Send(request);
        }

        [HttpPut("[action]")]
        public async Task<Result<CartItemResponse>> UpdateCartItem(UpdateCartItemRequest request)
        {
            return await _MediatR.Send(request);
        }

        [HttpPut("[action]/{CartItemID}")]
        public async Task<Result<bool>> DeleteCartItem(Guid CartItemID)
        {
            return await _MediatR.Send(new DeleteCartItemRequest(CartItemID));
        }
    }
}
