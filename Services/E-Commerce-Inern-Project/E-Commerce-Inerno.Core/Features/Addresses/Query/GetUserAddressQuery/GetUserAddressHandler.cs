using AutoMapper;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IAddressRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AddressDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Addresses.Query.GetUserAddressQuery
{
    public class GetUserAddressHandler : IRequestHandler<GetUserAddressQ, Result<IEnumerable<AddressDetails>>>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        public GetUserAddressHandler(IAddressRepository addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<AddressDetails>>> Handle(GetUserAddressQ request, CancellationToken cancellationToken)
        {
            var result = await _addressRepository.GetAddressesByUserId(request.UserID);
            if (!result.Any())
            {
                return Result<IEnumerable<AddressDetails>>.NotFound("No Address Was Found For THis User");
            }

            return Result<IEnumerable<AddressDetails>>.Success(result.Select(a => _mapper.Map<AddressDetails>(a)));
        }
    }
}
