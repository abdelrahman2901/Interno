using E_Commerce_Inern_Project.Core.DTO.AuthDTO;
using E_Commerce_Inern_Project.Core.Common;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<Result<AuthTokenResponse>>
    {
        public string? Email { get; set; } 
        public string? Password { get; set; } 
    }
    
}
