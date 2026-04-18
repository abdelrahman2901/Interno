using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.AddressDTO;
using MediatR;
namespace E_Commerce_Inern_Project.Core.Features.Address.Commands.UpdateAddressCmd
{
    public record UpdateAddressRequest(Guid AddressID, string AddressLabel, string MainAddress, string? BackUpAddress,
     Guid CityID, Guid AreaID, string BackUpPhoneNumber,bool IsDeleted, bool IsDefault) : IRequest<Result<AddressResponse>>;
}
