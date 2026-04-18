using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.AddressDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAddressServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Address.Commands.UpdateAddressCmd
{
    public class UpdateAddressHandler : IRequestHandler<UpdateAddressRequest, Result<AddressResponse>>
    {
        private readonly IAddressService _addressService;
        public UpdateAddressHandler(IAddressService addressService)
        {
            _addressService = addressService;
        }
        public async Task<Result<AddressResponse>> Handle(UpdateAddressRequest request, CancellationToken cancellationToken)
        {
            return await _addressService.UpdateAddress(request);
        }
    }
}
