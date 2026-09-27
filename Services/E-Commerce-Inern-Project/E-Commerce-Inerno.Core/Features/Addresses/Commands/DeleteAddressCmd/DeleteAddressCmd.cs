using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Addresses.Commands.DeleteAddressCmd
{
    public record DeleteAddressCmd(Guid AddressID) : IRequest<Result<bool>>;
}
