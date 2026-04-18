using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAuthServices;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.SignOutUser
{
    public class SignOutUserHandler : IRequestHandler<SignOutUserCommand, Result<bool>>
    {
        private readonly IAuthService _AuthService;
        public SignOutUserHandler(IAuthService authService)
        {
            _AuthService = authService;
        }

        public async Task<Result<bool>> Handle(SignOutUserCommand request, CancellationToken cancellationToken)
        {
            return await _AuthService.SignOut(request.UserID);
        }
    }
}
