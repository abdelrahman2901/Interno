using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Features.BannerSlide.Command.CreateBannerCmd;
using E_Commerce_Inern_Project.Core.Features.BannerSlide.Command.DeleteBannerCmd;
using E_Commerce_Inern_Project.Core.Features.BannerSlide.Command.ToggleBannerActiviationCmd;
using E_Commerce_Inern_Project.Core.Features.BannerSlide.Command.UpdateBannerCmd;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.BannerControl.Commands
{
    [Route("api/Banner")]
    [ApiController]
    public class BannerCommandsController : ControllerBase
    {
        private readonly IMediator _MediatR;

        public BannerCommandsController(IMediator mediator)
        {
            _MediatR = mediator;
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<Result<bool>> CreateBanner([FromForm] CreateBannerRequest request)
        {
            return await _MediatR.Send(request);
        }

        [HttpPut("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<Result<bool>> UpdateBanner([FromForm] UpdateBannerRequest request)
        {
            return await _MediatR.Send(request);
        }

        [HttpPut("[action]/{BannerID}")]
        [Authorize(Roles = "Admin")]
        public async Task<Result<bool>> DeleteBanner(Guid BannerID)
        {
            return await _MediatR.Send(new DeleteBannerRequest(BannerID));
        }

        [HttpPut("[action]/{BannerID}")]
        [Authorize(Roles = "Admin")]
        public async Task<Result<bool>> ToggleBannerActiviation(Guid BannerID)
        {
            return await _MediatR.Send(new ToggleBannerActiviationRequest(BannerID));
        }
    }
}
