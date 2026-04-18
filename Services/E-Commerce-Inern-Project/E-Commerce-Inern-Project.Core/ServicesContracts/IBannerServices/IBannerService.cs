using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.BannerSlideDTO;
using E_Commerce_Inern_Project.Core.Features.BannerSlide.Command.CreateBannerCmd;
using E_Commerce_Inern_Project.Core.Features.BannerSlide.Command.UpdateBannerCmd;

namespace E_Commerce_Inern_Project.Core.ServicesContracts.IBannerServices
{
    public interface IBannerService
    {
        public Task<Result<IEnumerable<BannerSlideResponse>>> GetBanners();
        public Task<Result<BannerSlideResponse>> GetBannerSlideByID(Guid BannerID);
        public Task<Result<bool>> CreateBanner(CreateBannerRequest request);
        public Task<Result<bool>> UpdateBanner(UpdateBannerRequest request);
        public Task<Result<bool>> DeleteBanner(Guid BannerID);
        public Task<Result<bool>> ToggleBannerActiviation(Guid BannerID);
    }
}
