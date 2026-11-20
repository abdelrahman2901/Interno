using E_Commerce_Inerno.Core.Features.WishLists.Query.GetAllWishListsQ;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.WishListDTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inerno.API.Controllers.WIshListControl.Queries
{
    [Route("api/WishList")]
    [ApiController]
    [Authorize]
    public class WishListQueriesController : ControllerBase
    {
        private readonly IMediator _MediatR;

        public WishListQueriesController(IMediator mediatR)
        {
            _MediatR = mediatR;
        }

        [HttpGet("GetAllUserWishList/{UserID}")]
        public async Task<Result<IEnumerable<WishListDetailsResponse>>> GetAllUserWishList(Guid UserID)
        {
            return await _MediatR.Send(new GetAllUserWishListQuery(UserID));
        }
    }
}
