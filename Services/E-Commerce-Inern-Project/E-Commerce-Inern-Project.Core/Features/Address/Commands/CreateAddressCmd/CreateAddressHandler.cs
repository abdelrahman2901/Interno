using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.AddressDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAddressServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Address.Commands.CreateAddressCmd
{
    public class CreateAddressHandler : IRequestHandler<CreateAddressRequest, Result<AddressResponse>>
    {
        private readonly IAddressService _addressService;
        public CreateAddressHandler(IAddressService addressService)
        {
            _addressService = addressService;
        }

        public async Task<Result<AddressResponse>> Handle(CreateAddressRequest request, CancellationToken cancellationToken)
        {
            return await _addressService.AddAddress(request);
        }
    }
}
