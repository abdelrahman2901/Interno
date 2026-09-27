using E_Commerce_Inern_Project.Core.RepositoryContracts.IAuthRepo;
using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.CheckPassword
{
    public class CheckPasswordHandler : IRequestHandler<CheckPasswordCommand, Result<bool>>
    {
        private readonly IAuthRepository _AuthRepo;

        public CheckPasswordHandler(IAuthRepository AuthRepo)
        {
            _AuthRepo = AuthRepo;
        }

        public async Task<Result<bool>> Handle(CheckPasswordCommand request, CancellationToken cancellationToken)
        {
            bool result = await _AuthRepo.IsPasswordCorrect(request.UserID, request.Password);
            if (!result)
            {
                return Result<bool>.BadRequest("Wrong Password");
            }
            return Result<bool>.Success(result);
        }
    }
}
