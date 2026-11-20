using E_Commerce_Interno.Domain.DTO.UserDTO;
using E_Commerce_Interno.Domain.Common;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;


namespace E_Commerce_Inerno.Core.Features.Auth.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<Result<AuthUserDetailsDTO>>
    {
        [JsonIgnore]
        [SwaggerSchema(Nullable = true)]
        public Guid UserID { get; set; }
        public string? Email { get; set; }
        public string? PersonName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
