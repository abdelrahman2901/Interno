using AutoMapper;
using E_Commerce_Inerno.Core.Domain.RepositoryContracts.IAddressRepo;
using E_Commerce_Inerno.Core.Communication;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AddressDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;
namespace E_Commerce_Inerno.Core.Features.Addresses.Commands.CreateAddressCmd
{
    public class CreateAddressHandler : IRequestHandler<CreateAddressRequest, Result<AddressResponse>>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly IInternoEventPublisher _Publisher;
        public CreateAddressHandler(IAddressRepository addressRepository, IInternoEventPublisher Publisher, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _Publisher = Publisher;
        }

        public async Task<Result<AddressResponse>> Handle(CreateAddressRequest request, CancellationToken cancellationToken)
        {
            var Address = _mapper.Map<Address>(request);
            var result = await _addressRepository.AddAddress(Address);
            if (!result)
            {
                return Result<AddressResponse>.InternalError("Failed To Add Address");
            }
            if (request.IsDefault)
            {
                Address? defaultAddress = await _addressRepository.GetDefaultAddress(true);
                defaultAddress?.IsDefault = false;

            }
            if (!await _addressRepository.SaveChanges())
            {
                return Result<AddressResponse>.InternalError("Failed To Save Changes");

            }

            string json = JsonSerializer.Serialize<Address>(Address);
            AuditRequest auditRequest = new(request.UserID ?? Guid.Empty, ActionTypeEnum.Create, nameof(Address), null, json);
            await _Publisher.PublishAuditAsync(auditRequest);

            return Result<AddressResponse>.Success(_mapper.Map<AddressResponse>(result));
        }
    }
}
