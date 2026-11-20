using AutoMapper;
using E_Commerce_Inerno.Core.Domain.RepositoryContracts.IAddressRepo;
using E_Commerce_Inerno.Core.Communication;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;

namespace E_Commerce_Inerno.Core.Features.Addresses.Commands.DeleteAddressCmd
{
    public class DeleteAddressHandler : IRequestHandler<DeleteAddressCmd, Result<bool>>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly IInternoEventPublisher _Publisher;
        
        public DeleteAddressHandler(IAddressRepository addressRepository, IInternoEventPublisher Publisher, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _Publisher = Publisher;
        }
        public async Task<Result<bool>> Handle(DeleteAddressCmd request, CancellationToken cancellationToken)
        {
            var result = await _addressRepository.GetAddressByID(request.AddressID, true);
            if (result == null)
            {
                return Result<bool>.NotFound("Address Doesnt Exists");
            }
            result.IsDeleted = true;

            if (!await _addressRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To Save Changes");

            }
            string json = JsonSerializer.Serialize<Address>(result);
            AuditRequest auditRequest = new(result.UserID, ActionTypeEnum.Delete, nameof(Address), json, null);
            await _Publisher.PublishAuditAsync(auditRequest);


            return Result<bool>.Success(result.IsDeleted);
        }
    }
}
