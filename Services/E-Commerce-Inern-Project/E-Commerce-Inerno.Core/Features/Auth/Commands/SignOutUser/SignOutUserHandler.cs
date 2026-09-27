using E_Commerce_Inern_Project.Core.Policies;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Polly;

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.SignOutUser
{
    public class SignOutUserHandler : IRequestHandler<SignOutUserCommand, Result<bool>>
    {
        private readonly IUserRepository _UserRepo;
        private readonly IRabbitMQPublisher _Publish;
        private readonly IAsyncPolicy _policy;
        private readonly string AuditRoutingKey = "Interno.Audit";

        public SignOutUserHandler(IPollyPolicy asyncPolicy, IRabbitMQPublisher Publish, IUserRepository UserRepo)
        {
            _UserRepo = UserRepo;
            _Publish = Publish;
            _policy = asyncPolicy.GetPolicy();
        }

        public async Task<Result<bool>> Handle(SignOutUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _UserRepo.GetApplicationUserByID(request.UserID, true);
            if (user == null)
            {
                return Result<bool>.NotFound("User not found");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpirtation = null;

            if (!await _UserRepo.SaveChanges())
            {
                return Result<bool>.InternalError("Failed to sign out user");
            }

            AuditRequest Request = new(request.UserID, ActionTypeEnum.Logout, nameof(ApplicationUser), null, null);

            await _policy.ExecuteAsync(async () =>
            {
                await _Publish.Publish(AuditRoutingKey, Request);
            });
            return Result<bool>.Success(true);
        }
    }
}
