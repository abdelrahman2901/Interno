using E_Commerce_Inern_Project.Core.Features.WishLists.Commands.ClearUserWishListCmd;
using E_Commerce_Inern_Project.Core.Features.WishLists.Commands.CreateWishListCmd;
using E_Commerce_Inern_Project.Core.Features.WishLists.Commands.DeleteWishListCmd;
using E_Commerce_Inern_Project.Core.Features.WishLists.Commands.UpdateWishListCmd;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.WishListDTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.WIshListControl.Commands
{
    [Route("api/WishList")]
    [ApiController]
    [Authorize]
    public class WishListCommandsController : ControllerBase
    {
        private readonly IMediator _MediatR;

        public WishListCommandsController(IMediator mediatR)
        {
            _MediatR = mediatR;
        }

        [HttpPost("CreateWishList")]
        public async Task<Result<bool>> CreateWishList(CreateWishListRequest request)
        {
            return await _MediatR.Send(request);
        }

        [HttpPut("UpdateWishList")]
        public async Task<Result<WishListResponse>> UpdateWishList(UpdateWishListRequest request)
        {
            return await _MediatR.Send(request);
        }

        [HttpPut("DeleteWishList/{WishListID}")]
        public async Task<Result<bool>> DeleteWishList(Guid WishListID)
        {
            return await _MediatR.Send(new DeleteWishListRequest(WishListID));
        }

        [HttpPut("ClearUserWishList/{UserID}")]
        public async Task<Result<bool>> ClearUserWishList(Guid UserID)
        {
            return await _MediatR.Send(new ClearUserWIshListRequest(UserID));
        }
    }
}
