using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AuthDTO;
using MediatR;


namespace E_Commerce_Inerno.Core.Features.Auth.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<Result<AuthTokenResponse>>
    {
        public string? Email { get; set; } 
        public string? Password { get; set; } 
    }
    
}
