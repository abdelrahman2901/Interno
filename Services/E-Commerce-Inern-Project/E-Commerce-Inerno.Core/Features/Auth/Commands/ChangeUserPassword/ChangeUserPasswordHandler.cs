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
using E_Commerce_Inerno.Core.Features.Auth.Commands.ChangeUserPassword;

namespace E_Commerce_Inerno.Core.Features.Auth.Commands.ChangeUserPassword
{
    public class ChangeUserPasswordHandler : IRequestHandler<ChangeUserPasswordCommand, Result<bool>>
    {
        private readonly IAuthRepository _AuthRepo;
        private readonly IUserRepository _UserRepo;
        private readonly IInternoEventPublisher _Publish;
        private readonly IAsyncPolicy _policy;
        private readonly string AuditRoutingKey = "Interno.Audit";

        public ChangeUserPasswordHandler(IAuthRepository AuthRepo, IPollyPolicy asyncPolicy, IInternoEventPublisher Publish, IUserRepository UserRepo)
        {
            _AuthRepo = AuthRepo;
            _UserRepo = UserRepo;
            _Publish = Publish;
            _policy = asyncPolicy.GetPolicy();
        }

        public async Task<Result<bool>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await _UserRepo.GetApplicationUserByID(request.UserID, true);
            if (user == null)
            {
                return Result<bool>.NotFound("User not found");
            }
            if (!await _AuthRepo.IsPasswordCorrect(user.UserID, request.CurrentPassword))
            {
                return Result<bool>.BadRequest("Incorrect current password");
            }

            string jsonOldValues = JsonSerializer.Serialize<ApplicationUser>(user);

            user.Password = request.NewPassword;

            if (!await _AuthRepo.SaveChanges())
            {
                return Result<bool>.BadRequest("Failed to change password");
            }
            string jsonNewValues = JsonSerializer.Serialize<ApplicationUser>(user);
            AuditRequest AuditRequest = new(user.UserID, ActionTypeEnum.ChangePassword, nameof(ApplicationUser), jsonOldValues, jsonNewValues);

            await _policy.ExecuteAsync(async () =>
            {
                await _Publish.PublishAuditAsync(AuditRequest);
            });
            return Result<bool>.Success(true);
        }
    }
}
