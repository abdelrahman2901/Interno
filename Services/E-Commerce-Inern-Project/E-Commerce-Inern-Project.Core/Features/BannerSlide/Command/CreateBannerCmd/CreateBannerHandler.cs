using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IBannerServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.BannerSlide.Command.CreateBannerCmd
{
    public class CreateBannerHandler : IRequestHandler<CreateBannerRequest, Result<bool>>
    {
        private readonly IBannerService _bannerService;
        public CreateBannerHandler(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }
        public async Task<Result<bool>> Handle(CreateBannerRequest request, CancellationToken cancellationToken)
        {
            return await _bannerService.CreateBanner(request);
        }
    }
}
