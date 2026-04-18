using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAddressServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Address.Commands.DeleteAddressCmd
{
    public class DeleteAddressHandler : IRequestHandler<DeleteAddressCmd, Result<bool>>
    {
        private readonly IAddressService _addressService;
        public DeleteAddressHandler(IAddressService addressService)
        {
            _addressService = addressService;
        }
        public async Task<Result<bool>> Handle(DeleteAddressCmd request, CancellationToken cancellationToken)
        {
            return await _addressService.DeleteAddress(request.AddressID);
        }
    }
}
