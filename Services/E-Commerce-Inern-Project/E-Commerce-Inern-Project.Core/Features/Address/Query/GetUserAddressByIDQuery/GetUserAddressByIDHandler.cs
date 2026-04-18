using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.AddressDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAddressServices;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.Address.Query.GetUserAddressByIDQuery
{
    public class GetUserAddressByIDHandler : IRequestHandler<GetUserAddressByIDQ, Result<AddressResponse>>
    {
        private readonly IAddressService _AddressDetails;
        public GetUserAddressByIDHandler(IAddressService AddressDetails)
        {
        _AddressDetails = AddressDetails;
        }   

        public async Task<Result<AddressResponse>> Handle(GetUserAddressByIDQ request, CancellationToken cancellationToken)
        {
            return await _AddressDetails.GetAddressByUserID(request.AddressID);
        }
    }
}
