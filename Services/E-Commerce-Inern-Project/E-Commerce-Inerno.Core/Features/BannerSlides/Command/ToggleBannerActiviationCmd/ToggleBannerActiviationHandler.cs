using E_Commerce_Inerno.Core.Communication;

using E_Commerce_Inerno.Core.RepositoryContracts.IBannerRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;

namespace E_Commerce_Inerno.Core.Features.BannerSlides.Command.ToggleBannerActiviationCmd
{
    public class ToggleBannerActiviationHandler : IRequestHandler<ToggleBannerActiviationRequest, Result<bool>>
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IInternoEventPublisher _Publisher;
        private readonly IUserRepository _UserRepo;
        

        public ToggleBannerActiviationHandler(IBannerRepository bannerRepository, IUserRepository UserRepo, IInternoEventPublisher Publisher)
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
            await _Publisher.PublishAuditAsync(AuditRequest);

            return Result<bool>.Success(true);
        }
    }
}
