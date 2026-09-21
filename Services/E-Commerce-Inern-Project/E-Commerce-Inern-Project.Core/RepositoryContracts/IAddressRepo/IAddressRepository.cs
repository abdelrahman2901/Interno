using E_Commerce_Inern_Project.Core.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IAddressRepo
{
    public interface IAddressRepository
    {
        Task< Address> GetAddressByID_NoTracking(Guid AddressID);
        Task< Address> GetAddressByID_Tracking(Guid AddressID);
        Task< Address> GetDefaultAddress_Tracking();
        
        Task<IEnumerable<Address>> GetAddressesByUserId(Guid userId);
        Task<Address> AddAddress(Address address);
        Task<bool> SaveChanges();
    }
}
