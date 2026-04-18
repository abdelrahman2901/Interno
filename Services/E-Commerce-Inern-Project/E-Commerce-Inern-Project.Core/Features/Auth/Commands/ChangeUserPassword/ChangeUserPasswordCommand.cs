using E_Commerce_Inern_Project.Core.Common;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.ChangeUserPassword
{
    public class ChangeUserPasswordCommand : IRequest<Result<bool>>
    {
        public Guid? UserID { get; set; }
        public string? CurrentPassword { get; set; } 
        public string? NewPassword { get; set; } 
        public string? confirmPassword { get; set; } 
    }
     
}
