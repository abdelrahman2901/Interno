using E_Commerce_Inerno.Core.Policies;
using E_Commerce_Inerno.Core.Communication;

using E_Commerce_Inerno.Core.RepositoryContracts.IAuthRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Polly;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;

namespace E_Commerce_Inerno.Core.Features.Auth.Commands.UserResetPassword
{
    public class UserResetPasswordHandler : IRequestHandler<UserResetPasswordCommand, Result<bool>>
    {
        private readonly IAuthRepository _AuthRepo;
        private readonly IUserRepository _UserRepo;
        private readonly IInternoEventPublisher _Publish;
        private readonly IAsyncPolicy _policy;
        private readonly string AuditRoutingKey = "Interno.Audit";

        public UserResetPasswordHandler(IAuthRepository AuthRepo, IPollyPolicy asyncPolicy, IInternoEventPublisher Publish, IUserRepository UserRepo)
        {
            _AuthRepo = AuthRepo;
            _UserRepo = UserRepo;
            _Publish = Publish;
            _policy = asyncPolicy.GetPolicy();
        }

        public async Task<Result<bool>> Handle(UserResetPasswordCommand request, CancellationToken cancellationToken)
        {
            ApplicationUser? User = await _UserRepo.GetApplicationUserByID(request.UserID, true);
            if (User == null)
            {
                return Result<bool>.NotFound("User not found");
            }
            string jsonOldValues = JsonSerializer.Serialize<ApplicationUser>(User);
            User.Password = request.NewPassword;

            if (!await _AuthRepo.SaveChanges())
            {
                return Result<bool>.BadRequest("Failed to change password");
            }

            string jsonNewValues = JsonSerializer.Serialize<ApplicationUser>(User);

            AuditRequest Request = new(User.UserID, ActionTypeEnum.ResetPassword, nameof(ApplicationUser), jsonOldValues, jsonNewValues);

            await _policy.ExecuteAsync(async () =>
            {
                await _Publish.PublishAuditAsync(Request);
            });

            return Result<bool>.Success(true);
        }
    }
}
