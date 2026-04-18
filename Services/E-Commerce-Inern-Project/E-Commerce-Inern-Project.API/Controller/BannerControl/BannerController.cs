using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.BannerSlideDTO;
using E_Commerce_Inern_Project.Core.Features.BannerSlide.Command.CreateBannerCmd;
using E_Commerce_Inern_Project.Core.Features.BannerSlide.Command.DeleteBannerCmd;
using E_Commerce_Inern_Project.Core.Features.BannerSlide.Command.ToggleBannerActiviationCmd;
using E_Commerce_Inern_Project.Core.Features.BannerSlide.Command.UpdateBannerCmd;
using E_Commerce_Inern_Project.Core.Features.BannerSlide.Query.GetAllBannersQ;
using E_Commerce_Inern_Project.Core.Features.BannerSlide.Query.GetBannerByIDQ;
using E_Commerce_Inern_Project.Core.ServicesContracts.IBannerServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controller.BannerControl
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly IMediator _MediatR;
        public BannerController(IMediator mediator)
        {
            _MediatR = mediator;
        }
       
        [HttpGet]
        public async Task<Result<IEnumerable<BannerSlideResponse>>> GetAllBanners()
        {
            return await _MediatR.Send(new GetAllBannersQuery());
        }
       
        [HttpGet("{BannerID}")]
        public async Task<Result<BannerSlideResponse>> GetBannnerByID(Guid BannerID)
        {
            return await _MediatR.Send(new GetBannerByIDQuery(BannerID));
        }
        
        [HttpPost]
        public async Task<Result<bool>> CreateBanner([FromForm]CreateBannerRequest request)
        {
            return await _MediatR.Send(request);
        }
        
        [HttpPut]
        public async Task<Result<bool>> UpdateBanner([FromForm] UpdateBannerRequest request)
        {
            return await _MediatR.Send(request);
        }

        [HttpPut("DeleteBanner/{BannerID}")]
        public async Task<Result<bool>> DeleteBanner(Guid BannerID)

        {
            return await _MediatR.Send(new DeleteBannerRequest(BannerID));
        }
        [HttpPut("ToggleBannerActiviation/{BannerID}")]
        public async Task<Result<bool>> ToggleBannerActiviation(Guid BannerID)
        {
            return await _MediatR.Send(new ToggleBannerActiviationRequest(BannerID  ));
        }
    }
}
