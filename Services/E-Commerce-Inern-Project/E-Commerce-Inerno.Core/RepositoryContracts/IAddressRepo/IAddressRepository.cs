using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IAddressRepo
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetAddressesByUserId(Guid userId);
        Task<Address?> GetAddressByID(Guid AddressID, bool AsTracking);
        Task<Address?> GetDefaultAddress(bool AsTracking);
        Task<bool> AddAddress(Address address);
        Task<bool> SaveChanges();
    }
}
