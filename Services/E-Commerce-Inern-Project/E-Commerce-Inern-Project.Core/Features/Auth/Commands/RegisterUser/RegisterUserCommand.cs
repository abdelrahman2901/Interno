using E_Commerce_Inern_Project.Core.DTO.AuthDTO;
using E_Commerce_Inern_Project.Core.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<Result<AuthTokenResponse>>
    {
        public string? UserName { get; set; } 
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
