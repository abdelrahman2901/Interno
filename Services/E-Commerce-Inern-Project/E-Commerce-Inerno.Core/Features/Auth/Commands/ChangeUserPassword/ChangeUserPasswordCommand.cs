using E_Commerce_Interno.Domain.Common;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;


namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.ChangeUserPassword
{
    public class ChangeUserPasswordCommand : IRequest<Result<bool>>
    {
        [JsonIgnore]
        [SwaggerSchema(Nullable = true)]
        public Guid? UserID { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
    }

}
