using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AddressDTO;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;


namespace E_Commerce_Inerno.Core.Features.Addresses.Commands.CreateAddressCmd
{
    public class CreateAddressRequest : IRequest<Result<AddressResponse>>
    {
        public string AddressLabel { get; set; }
        [JsonIgnore]
        [SwaggerSchema(Nullable =true)]
        public Guid? UserID { get; set; }
        public string MainAddress { get; set; }
        public string? BackUpAddress { get; set; }
        public Guid CityID { get; set; }
        public Guid AreaID { get; set; }
        public string BackUpPhoneNumber { get; set; }
        public bool IsDefault { get; set; }
    }
}
