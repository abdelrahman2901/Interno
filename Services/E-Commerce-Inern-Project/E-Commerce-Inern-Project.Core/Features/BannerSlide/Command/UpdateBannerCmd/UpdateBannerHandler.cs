using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IBannerServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.BannerSlide.Command.UpdateBannerCmd
{
    public class UpdateBannerHandler : IRequestHandler<UpdateBannerRequest, Result<bool>>
    {
        private readonly IBannerService _bannerService;
        public UpdateBannerHandler(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }
        public async Task<Result<bool>> Handle(UpdateBannerRequest request, CancellationToken cancellationToken)
        {
            return await _bannerService.UpdateBanner(request);
        }
    }
}
