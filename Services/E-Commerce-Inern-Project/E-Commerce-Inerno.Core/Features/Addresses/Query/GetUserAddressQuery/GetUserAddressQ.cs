using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AddressDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Addresses.Query.GetUserAddressQuery
{
    public record GetUserAddressQ(Guid UserID) : IRequest<Result<IEnumerable<AddressDetails>>>;
}
