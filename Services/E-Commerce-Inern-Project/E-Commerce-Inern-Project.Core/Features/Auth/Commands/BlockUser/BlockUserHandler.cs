using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAuthServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.BlockUser
{
    public class BlockUserHandler : IRequestHandler<BlockUserCommand, Result<bool>>
    {
        private readonly IAuthService _AuthService;
        public BlockUserHandler(IAuthService authService)
        {
            _AuthService = authService;
        }

        public async Task<Result<bool>> Handle(BlockUserCommand request, CancellationToken cancellationToken)
        {
            return await _AuthService.BlockUser(request.UserID);
        }
    }
}
