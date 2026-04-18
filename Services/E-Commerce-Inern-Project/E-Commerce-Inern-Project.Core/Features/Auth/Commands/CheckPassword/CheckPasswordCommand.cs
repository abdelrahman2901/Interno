using E_Commerce_Inern_Project.Core.Common;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.CheckPassword
{
   public class CheckPasswordCommand :IRequest<Result<bool>>
    {
        public Guid UserID { get; set; }
        public string Password { get; set; }
    }
}
