using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.BannerSlideDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IBannerServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.BannerSlide.Query.GetBannerByIDQ
{
    public class GetBannerByIDHandler : IRequestHandler<GetBannerByIDQuery, Result<BannerSlideResponse>>
    {
        private readonly IBannerService _bannerService;
        public GetBannerByIDHandler(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }
        public async Task<Result<BannerSlideResponse>> Handle(GetBannerByIDQuery request, CancellationToken cancellationToken)
        {
            return await _bannerService.GetBannerSlideByID(request.BannerID);
        }
    }
}
