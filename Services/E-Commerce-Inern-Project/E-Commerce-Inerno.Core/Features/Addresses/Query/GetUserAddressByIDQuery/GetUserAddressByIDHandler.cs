using AutoMapper;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IAddressRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AddressDTO;
using MediatR;


namespace E_Commerce_Inern_Project.Core.Features.Addresses.Query.GetUserAddressByIDQuery
{
    public class GetUserAddressByIDHandler : IRequestHandler<GetUserAddressByIDQ, Result<AddressResponse>>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        public GetUserAddressByIDHandler(IAddressRepository addressRepository,IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }


        public async Task<Result<AddressResponse>> Handle(GetUserAddressByIDQ request, CancellationToken cancellationToken)
        {
            var result = await _addressRepository.GetAddressByID(request.AddressID, false);
            if (result == null)
            {
                return Result<AddressResponse>.NotFound("Address Doesnt Exists");
            }
            return Result<AddressResponse>.Success(_mapper.Map<AddressResponse>(result));
        }
    }
}
