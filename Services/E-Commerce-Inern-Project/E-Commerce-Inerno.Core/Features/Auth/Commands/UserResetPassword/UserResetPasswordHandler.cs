using E_Commerce_Inern_Project.Core.Policies;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IAuthRepo;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Polly;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.UserResetPassword
{
    public class UserResetPasswordHandler : IRequestHandler<UserResetPasswordCommand, Result<bool>>
    {
        private readonly IAuthRepository _AuthRepo;
        private readonly IUserRepository _UserRepo;
        private readonly IRabbitMQPublisher _Publish;
        private readonly IAsyncPolicy _policy;
        private readonly string AuditRoutingKey = "Interno.Audit";

        public UserResetPasswordHandler(IAuthRepository AuthRepo, IPollyPolicy asyncPolicy, IRabbitMQPublisher Publish, IUserRepository UserRepo)
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
                await _Publish.Publish(AuditRoutingKey, Request);
            });

            return Result<bool>.Success(true);
        }
    }
}
