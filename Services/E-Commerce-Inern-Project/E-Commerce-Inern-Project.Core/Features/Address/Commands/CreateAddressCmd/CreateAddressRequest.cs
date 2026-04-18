using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.AddressDTO;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.Address.Commands.CreateAddressCmd
{
    public record CreateAddressRequest( string AddressLabel,Guid UserID ,string MainAddress, string? BackUpAddress,
        Guid CityID, Guid AreaID, string BackUpPhoneNumber,bool IsDefault) : IRequest<Result<AddressResponse>>;
}
