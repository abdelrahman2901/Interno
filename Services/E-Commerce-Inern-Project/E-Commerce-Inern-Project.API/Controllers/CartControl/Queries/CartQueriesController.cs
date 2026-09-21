using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CartDTO;
using E_Commerce_Inern_Project.Core.Features.Cart.Query.GetUserCartDetailsQ;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce_Inern_Project.API.Controllers.CartControl.Queries
{
    [AllowAnonymous]
    [Route("api/Cart")]
    [ApiController]
    public class CartQueriesController : ControllerBase
    {
        private readonly IMediator _MediatR;

        public CartQueriesController(IMediator mediatR)
        {
            _MediatR = mediatR;
        }

        [HttpGet("[action]")]
        public async Task<Result<CartDetailsResponse>> GetUserCartItemsDetails()
        {
            var UserStrID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(UserStrID))
            {
                return Result<CartDetailsResponse>.UnAuthorized("User not Authorized");
            }

            var UserID = Guid.Parse(UserStrID);
            return await _MediatR.Send(new GetUserCarItemsQuery(UserID));
        }
    }
}
