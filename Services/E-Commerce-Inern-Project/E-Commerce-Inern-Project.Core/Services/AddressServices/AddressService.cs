using AutoMapper;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IAddressRepo;
using E_Commerce_Inern_Project.Core.DTO.AddressDTO;
using E_Commerce_Inern_Project.Core.Features.Address.Commands.CreateAddressCmd;
using E_Commerce_Inern_Project.Core.Features.Address.Commands.UpdateAddressCmd;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAddressServices;
using MediatR;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Services.AddressServices
{
    public class AddressService : IAddressService
    { 
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly string _AuditRoutingKey = "Interno.Audit";
        public AddressService(IAddressRepository addressRepository, IRabbitMQPublisher Publisher, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _Publisher = Publisher;
        }

        public async Task<Result<AddressResponse>> AddAddress(CreateAddressRequest request)
        {
            var result = await _addressRepository.AddAddress(_mapper.Map<Address>(request));
            if (result == null)
            {
                return Result<AddressResponse>.InternalError("Failed To Add Address");
            }
            if (request.IsDefault)
            {
                Address? defaultAddress = await _addressRepository.GetDefaultAddress_Tracking();
                    defaultAddress?.IsDefault = false;

            }
            if (!await _addressRepository.SaveChanges())
            {
                return Result<AddressResponse>.InternalError("Failed To Save Changes");

            }

            string json=JsonSerializer.Serialize<Address>(result);
            AuditRequest AuditRequest = new(request.UserID, ActionTypeEnum.Create, nameof(Address), null, json);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            return Result<AddressResponse>.Success(_mapper.Map<AddressResponse>(result));
        }

        public async Task<Result<bool>> DeleteAddress(Guid AddressID)
        {
            var result = await _addressRepository.GetAddressByID_Tracking(AddressID);
            if (result == null)
            {
                return Result<bool>.NotFound("Address Doesnt Exists");
            }
            result.IsDeleted = true;

            if (!await _addressRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To Save Changes");

            }
            string json=JsonSerializer.Serialize<Address>(result);
            AuditRequest AuditRequest = new(result.UserID, ActionTypeEnum.Delete, nameof(Address), json, null);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);


            return Result<bool>.Success(result.IsDeleted);
        }

        public async Task<Result<AddressResponse>> GetAddressByUserID(Guid AddressID)
        {
            var result = await _addressRepository.GetAddressByID_NoTracking(AddressID);
            if (result == null)
            {
                return Result<AddressResponse>.NotFound("Address Doesnt Exists");
            }
            return Result<AddressResponse>.Success(_mapper.Map<AddressResponse>(result));
        }

        public async Task<Result<IEnumerable<AddressDetails>>> GetAddressesByUserId(Guid userId)
        {
            var result = await _addressRepository.GetAddressesByUserId(userId);
            if (!result.Any())
            {
                return Result<IEnumerable<AddressDetails>>.NotFound("No Address Was Found For THis User");
            }

            return Result<IEnumerable<AddressDetails>>.Success(result.Select(a=>_mapper.Map<AddressDetails>(a)));
        }

        public async Task<Result<AddressResponse>> UpdateAddress(UpdateAddressRequest request)
        {
            var result = await _addressRepository.GetAddressByID_Tracking(request.AddressID);
            if (result == null)
            {
                return Result<AddressResponse>.NotFound("Address Doesnt Exists");
            }

            if (request.IsDefault)
            {
                Address? defaultAddress = await _addressRepository.GetDefaultAddress_Tracking();
                if (defaultAddress != null && defaultAddress.AddressID!=request.AddressID)
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

            string jsonNewValue=JsonSerializer.Serialize<Address>(result);
            AuditRequest AuditRequest = new(result.UserID, ActionTypeEnum.Update, nameof(Address), jsonOldValue, jsonNewValue);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);


            return Result<AddressResponse>.Success(_mapper.Map<AddressResponse>(result));
        }
    }
}
