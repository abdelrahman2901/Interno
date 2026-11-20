using AutoMapper;
using E_Commerce_Inerno.Core.Domain.RepositoryContracts.IAddressRepo;
using E_Commerce_Inerno.Core.Communication;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AddressDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;

namespace E_Commerce_Inerno.Core.Features.Addresses.Commands.UpdateAddressCmd
{
    public class UpdateAddressHandler : IRequestHandler<UpdateAddressRequest, Result<AddressResponse>>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly IInternoEventPublisher _Publisher;
        public UpdateAddressHandler(IAddressRepository addressRepository, IInternoEventPublisher Publisher, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _Publisher = Publisher;
        }
        public async Task<Result<AddressResponse>> Handle(UpdateAddressRequest request, CancellationToken cancellationToken)
        {
            var result = await _addressRepository.GetAddressByID(request.AddressID, true);
            if (result == null)
            {
                return Result<AddressResponse>.NotFound("Address Doesnt Exists");
            }

            if (request.IsDefault)
            {
                Address? defaultAddress = await _addressRepository.GetDefaultAddress(true);
                if (defaultAddress != null && defaultAddress.AddressID != request.AddressID)
                {
                    defaultAddress.IsDefault = false;
                }
            }

            string jsonOldValue = JsonSerializer.Serialize<Address>(result);
            _mapper.Map(request, result);


            if (!await _addressRepository.SaveChanges())
            {
                return Result<AddressResponse>.InternalError("Failed To Save Changes");

            }

            string jsonNewValue = JsonSerializer.Serialize<Address>(result);
            AuditRequest auditRequest = new(result.UserID, ActionTypeEnum.Update, nameof(Address), jsonOldValue, jsonNewValue);
            await _Publisher.PublishAuditAsync(auditRequest);


            return Result<AddressResponse>.Success(_mapper.Map<AddressResponse>(result));
        }
    }
}
