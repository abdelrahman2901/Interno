using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.BannerSlideDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IBannerServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.BannerSlide.Query.GetAllBannersQ
{
    public class GetAllBannersHandler : IRequestHandler<GetAllBannersQuery, Result<IEnumerable<BannerSlideResponse>>>
    {
        private readonly IBannerService _bannerService;
        public GetAllBannersHandler(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

       public async Task<Result<IEnumerable<BannerSlideResponse>>> Handle(GetAllBannersQuery request, CancellationToken cancellationToken)
        {
            return await _bannerService.GetBanners();
        }
    }
}
