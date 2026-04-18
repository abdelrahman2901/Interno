using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IBannerServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.BannerSlide.Command.DeleteBannerCmd
{
    public class DeleteBannerHandler : IRequestHandler<DeleteBannerRequest, Result<bool>>
    {
        private readonly IBannerService _bannerService;
        public DeleteBannerHandler(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }
        public async Task<Result<bool>> Handle(DeleteBannerRequest request, CancellationToken cancellationToken)
        {
            return await _bannerService.DeleteBanner(request.BannerID);
        }
    }
}
