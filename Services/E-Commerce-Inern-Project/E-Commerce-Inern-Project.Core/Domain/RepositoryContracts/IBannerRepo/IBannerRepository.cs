using E_Commerce_Inern_Project.Core.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IBannerRepo
{
    public interface IBannerRepository
    {

        Task<IEnumerable<BannerSlide>> GetAllBanners();
        Task<BannerSlide?> GetBannerByID_NoTracking(Guid id);
        Task<BannerSlide?> GetBannerByID_Traking(Guid id);
        Task<bool> AddAsync(BannerSlide slide);
        Task<bool> SaveChanges();
    }
}
