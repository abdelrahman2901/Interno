using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.RepositoryContracts.IBannerRepo
{
    public interface IBannerRepository
    {

        Task<IEnumerable<BannerSlide>> GetAllBanners();
        Task<BannerSlide?> GetBannerByID(Guid id,bool AsTracking);
        Task<bool> AddAsync(BannerSlide slide);
        Task<bool> SaveChanges();
    }
}
