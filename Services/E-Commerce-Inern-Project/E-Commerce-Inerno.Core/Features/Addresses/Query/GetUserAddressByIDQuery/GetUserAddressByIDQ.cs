using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AddressDTO;
using MediatR;


namespace E_Commerce_Inerno.Core.Features.Addresses.Query.GetUserAddressByIDQuery
{
    public record GetUserAddressByIDQ(Guid AddressID) : IRequest<Result<AddressResponse>>;
    
}
