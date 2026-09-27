using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IBannerRepo;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.BannerSlides.Command.ToggleBannerActiviationCmd
{
    public class ToggleBannerActiviationHandler : IRequestHandler<ToggleBannerActiviationRequest, Result<bool>>
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly IUserRepository _UserRepo;
        private readonly string _AuditRoutingKey = "Interno.Audit";

        public ToggleBannerActiviationHandler(IBannerRepository bannerRepository, IUserRepository UserRepo, IRabbitMQPublisher Publisher)
        {
            _bannerRepository = bannerRepository;
            _Publisher = Publisher;
            _UserRepo = UserRepo;
        }

        public async Task<Result<bool>> Handle(ToggleBannerActiviationRequest request, CancellationToken cancellationToken)
        {
            var existing = await _bannerRepository.GetBannerByID(request.BannerID, true);
            if (existing == null)
            {
                return Result<bool>.NotFound("Banner Wasnt Found");
            }
            string JsonOldValues = JsonSerializer.Serialize<BannerSlide>(existing);
            existing.IsActive = !existing.IsActive;
            if (!await _bannerRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaveChanges");
            }

            string JsonNewValues = JsonSerializer.Serialize<BannerSlide>(existing);

            AuditRequest AuditRequest = new(await _UserRepo.GetAdminID(), ActionTypeEnum.Update, nameof(BannerSlide), JsonOldValues, JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            return Result<bool>.Success(true);
        }
    }
}
