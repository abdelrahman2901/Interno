using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.BannerSlideDTO;
using E_Commerce_Inern_Project.Core.Features.BannerSlide.Query.GetAllBannersQ;
using E_Commerce_Inern_Project.Core.Features.BannerSlide.Query.GetBannerByIDQ;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.BannerControl.Queries
{
    [Route("api/Banner")]
    [ApiController]
    public class BannerQueriesController : ControllerBase
    {
        private readonly IMediator _MediatR;

        public BannerQueriesController(IMediator mediator)
        {
            _MediatR = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<Result<IEnumerable<BannerSlideResponse>>> GetAllBanners()
        {
            return await _MediatR.Send(new GetAllBannersQuery());
        }

        [HttpGet("[action]/{BannerID}")]
        [Authorize(Roles = "Admin")]
        public async Task<Result<BannerSlideResponse>> GetBannnerByID(Guid BannerID)
        {
            return await _MediatR.Send(new GetBannerByIDQuery(BannerID));
        }
    }
}
