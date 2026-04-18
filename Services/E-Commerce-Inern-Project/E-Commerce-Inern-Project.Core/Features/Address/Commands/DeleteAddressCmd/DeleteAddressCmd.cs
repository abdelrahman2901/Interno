using E_Commerce_Inern_Project.Core.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Address.Commands.DeleteAddressCmd
{
    public record DeleteAddressCmd(Guid AddressID) : IRequest<Result<bool>>;
}
