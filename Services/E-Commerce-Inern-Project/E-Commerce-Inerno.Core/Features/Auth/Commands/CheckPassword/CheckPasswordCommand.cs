using E_Commerce_Interno.Domain.Common;
using MediatR;


namespace E_Commerce_Inerno.Core.Features.Auth.Commands.CheckPassword
{
   public class CheckPasswordCommand :IRequest<Result<bool>>
    {
        public Guid UserID { get; set; }
        public string Password { get; set; }
    }
}
