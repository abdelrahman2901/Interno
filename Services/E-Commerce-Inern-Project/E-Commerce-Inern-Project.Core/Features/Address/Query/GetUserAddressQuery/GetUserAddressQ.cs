using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.AddressDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Address.Query.GetUserAddressQuery
{
    public record GetUserAddressQ(Guid UserID) : IRequest<Result<IEnumerable<AddressDetails>>>;
}
