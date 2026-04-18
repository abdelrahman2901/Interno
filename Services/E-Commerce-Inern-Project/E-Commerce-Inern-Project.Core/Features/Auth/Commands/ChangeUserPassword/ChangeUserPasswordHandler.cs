using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAuthServices;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.ChangeUserPassword
{
    public class ChangeUserPasswordHandler : IRequestHandler<ChangeUserPasswordCommand, Result<bool>>
    {
        private readonly IAuthService _AuthService;
        public ChangeUserPasswordHandler(IAuthService authService)
        {
            _AuthService = authService;
        }
        public async Task<Result<bool>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {

            return await _AuthService.ChangeUserPassword(request);
        }
    }
}
