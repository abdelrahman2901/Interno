using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.AddressDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAddressServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Address.Query.GetUserAddressQuery
{
    public class GetUserAddressHandler : IRequestHandler<GetUserAddressQ, Result<IEnumerable<AddressDetails>>>
    {
        private readonly IAddressService _addressService;
        public GetUserAddressHandler(IAddressService addressService)
        {
            _addressService = addressService;
        }
        public async Task<Result<IEnumerable<AddressDetails>>> Handle(GetUserAddressQ request, CancellationToken cancellationToken)
        {
            return await _addressService.GetAddressesByUserId(request.UserID);
        }
    }
}
