using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IBannerServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.BannerSlide.Command.ToggleBannerActiviationCmd
{
    public class ToggleBannerActiviationHandler : IRequestHandler<ToggleBannerActiviationRequest, Result<bool>>
    {
        private readonly IBannerService _bannerService;
        public ToggleBannerActiviationHandler(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }
        public async Task<Result<bool>> Handle(ToggleBannerActiviationRequest request, CancellationToken cancellationToken)
        {
            return await _bannerService.ToggleBannerActiviation(request.BannerID);
        }
    }
}
