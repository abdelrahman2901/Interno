using AutoMapper;
using E_Commerce_Inerno.Core.Policies;
using E_Commerce_Inerno.Core.Communication;

using E_Commerce_Inerno.Core.RepositoryContracts.IAuthRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.UserDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Polly;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;

namespace E_Commerce_Inerno.Core.Features.Auth.Commands.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<AuthUserDetailsDTO>>
    {
        private readonly IAuthRepository _AuthRepo;
        private readonly IUserRepository _UserRepo;
        private readonly IInternoEventPublisher _Publish;
        private readonly IMapper _Mapper;
        private readonly IAsyncPolicy _policy;
        private readonly string AuditRoutingKey = "Interno.Audit";

        public UpdateUserHandler(IAuthRepository AuthRepo, IPollyPolicy asyncPolicy, IInternoEventPublisher Publish, IUserRepository UserRepo, IMapper Mapper)
        {
            _AuthRepo = AuthRepo;
            _UserRepo = UserRepo;
            _Mapper = Mapper;
            _Publish = Publish;
            _policy = asyncPolicy.GetPolicy();
        }

        public async Task<Result<AuthUserDetailsDTO>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await _UserRepo.GetApplicationUserByID(request.UserID, true);
            if (user == null)
            {
                return Result<AuthUserDetailsDTO>.NotFound("User not found");
            }

            string jsonOldValues = JsonSerializer.Serialize<ApplicationUser>(user);
            _Mapper.Map(request, user);

            var SaveResult = await _AuthRepo.SaveChanges();
            if (!SaveResult)
            {
                return Result<AuthUserDetailsDTO>.InternalError("Failed to update user");
            }

            AuthUserDetailsDTO UserDetails = _Mapper.Map<AuthUserDetailsDTO>(user);
            string jsonNewValues = JsonSerializer.Serialize<ApplicationUser>(user);
            AuditRequest Request = new(user.UserID, ActionTypeEnum.Update, nameof(ApplicationUser), jsonOldValues, jsonNewValues);

            await _policy.ExecuteAsync(async () =>
            {
                await _Publish.PublishAuditAsync(Request);
            });

            return Result<AuthUserDetailsDTO>.Success(UserDetails);
        }
    }
}
