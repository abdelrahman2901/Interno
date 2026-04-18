using E_Commerce_Inern_Project.Core.DTO.UserDTO;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAuthServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<AuthUserDetailsDTO?>>
    {
        private readonly IAuthService _AuthService;
        public UpdateUserHandler(IAuthService authService)
        {
            _AuthService = authService;
        }

        public async Task<Result<AuthUserDetailsDTO?>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            return await _AuthService.UpdateUser(request.UserUpdate);
        }
    }
}
