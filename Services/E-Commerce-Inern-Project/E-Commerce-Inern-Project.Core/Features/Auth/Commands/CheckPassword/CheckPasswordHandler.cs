using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAuthServices;
using MediatR;
 
namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.CheckPassword
{
    public class CheckPasswordHandler : IRequestHandler<CheckPasswordCommand, Result<bool>>
    {
        private readonly IAuthService _authservice;
        public CheckPasswordHandler(IAuthService authservice)
        {
            _authservice = authservice;
        }

        public async Task<Result<bool>> Handle(CheckPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _authservice.CheckPassword(request.UserID, request.Password);
        }

    }
}
