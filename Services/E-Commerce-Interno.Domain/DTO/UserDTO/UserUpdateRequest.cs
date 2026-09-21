using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace E_Commerce_Interno.Domain.DTO.UserDTO
{
    public class dUserUpdateRequest
    {
        [JsonIgnore]
        [SwaggerSchema(Nullable = true)]
        public Guid UserID { get; set; }
        public string? Email { get; set; }
        public string? PersonName { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
