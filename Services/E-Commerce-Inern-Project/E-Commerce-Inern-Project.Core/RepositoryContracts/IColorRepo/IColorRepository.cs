using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.DTO.SizeDTO;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IColorRepo
{
    public interface IColorRepository
    {
        public Task<Colors?> CreateColors(Colors color);
   
        public Task<IEnumerable<Colors>> GetAllColors();
        public Task<Colors?> GetColor(Guid ColorID);
        public Task<bool> SaveChanges();
    }
}
