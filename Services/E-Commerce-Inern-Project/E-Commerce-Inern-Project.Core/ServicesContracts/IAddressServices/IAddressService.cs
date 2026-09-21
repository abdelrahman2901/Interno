using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Features.Address.Commands.CreateAddressCmd;
using E_Commerce_Inern_Project.Core.Features.Address.Commands.UpdateAddressCmd;
using E_Commerce_Interno.Domain.DTO.AddressDTO;

namespace E_Commerce_Inern_Project.Core.ServicesContracts.IAddressServices
{
    public interface IAddressService
    {
        Task<Result<AddressResponse>> AddAddress(CreateAddressRequest request);
        Task<Result<AddressResponse>> UpdateAddress(UpdateAddressRequest request);
        Task<Result<bool>> DeleteAddress(Guid AddressID);
        Task<Result<IEnumerable<AddressDetails>>> GetAddressesByUserId(Guid userId);
        Task<Result<AddressResponse>> GetAddressByUserID(Guid AddressID);
    }
}
