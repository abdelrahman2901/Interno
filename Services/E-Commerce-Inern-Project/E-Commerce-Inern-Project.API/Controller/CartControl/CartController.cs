using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CartDTO;
using E_Commerce_Inern_Project.Core.Features.Cart.Commands.CreateCartCmd;
using E_Commerce_Inern_Project.Core.Features.Cart.Query.GetUserCartDetailsQ;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controller.CartControl
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMediator _MediatR;
        public CartController(IMediator mediatR)
        {
            _MediatR = mediatR;
        }
        [Authorize]
        [HttpGet("GetUserCartItemsDetails/{UserID}")] 
        public async Task<Result<CartDetailsResponse>> GetUserCartItemsDetails(Guid UserID)
        {
            return await _MediatR.Send(new GetUserCarItemsQuery(UserID));
        }

        [AllowAnonymous]
        [HttpPost("CreateCart")]
        public async Task<Result<CartResponse>> CreateCart(CreateCartRequest request)
        {
            return await _MediatR.Send(request);
        }
    }
}
