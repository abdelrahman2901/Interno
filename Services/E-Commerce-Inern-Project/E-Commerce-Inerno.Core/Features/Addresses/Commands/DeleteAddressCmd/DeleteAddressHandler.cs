using AutoMapper;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IAddressRepo;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.Addresses.Commands.DeleteAddressCmd
{
    public class DeleteAddressHandler : IRequestHandler<DeleteAddressCmd, Result<bool>>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly string _AuditRoutingKey = "Interno.Audit";
        public DeleteAddressHandler(IAddressRepository addressRepository, IRabbitMQPublisher Publisher, IMapper mapper)
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
            await _Publisher.Publish(_AuditRoutingKey, auditRequest);


            return Result<bool>.Success(result.IsDeleted);
        }
    }
}
