using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAuthServices;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Result<bool>>
    {
        private readonly IAuthService _AuthService;
        public DeleteUserHandler(IAuthService authService)
        {
            _AuthService = authService;
        }

        public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            return await _AuthService.DeleteUser(request.UserID);
        }
    }
}
