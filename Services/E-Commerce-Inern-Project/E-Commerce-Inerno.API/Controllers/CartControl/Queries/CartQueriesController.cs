using E_Commerce_Inerno.Core.Features.Carts.Query.GetUserCartDetailsQ;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CartDTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce_Inerno.API.Controllers.CartControl.Queries
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
