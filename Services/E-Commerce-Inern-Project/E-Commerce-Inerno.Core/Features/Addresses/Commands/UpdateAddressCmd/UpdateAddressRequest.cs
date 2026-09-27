using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AddressDTO;
using MediatR;
namespace E_Commerce_Inerno.Core.Features.Addresses.Commands.UpdateAddressCmd
{
    public class UpdateAddressRequest : IRequest<Result<AddressResponse>>
    {
        public Guid AddressID { get; }
        public string AddressLabel { get; }
        public string MainAddress { get; }
        public string? BackUpAddress { get; }
        public Guid CityID { get; }
        public Guid AreaID { get; }
        public string BackUpPhoneNumber { get; }
        public bool IsDeleted { get; }
        public bool IsDefault { get; }
    }
}
