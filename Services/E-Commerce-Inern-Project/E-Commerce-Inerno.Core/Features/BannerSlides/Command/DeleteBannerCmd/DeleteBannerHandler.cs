using E_Commerce_Inerno.Core.Domain.RepositoryContracts.ITransectionRepo;
using E_Commerce_Inerno.Core.Communication;

using E_Commerce_Inerno.Core.RepositoryContracts.IBannerRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;

namespace E_Commerce_Inerno.Core.Features.BannerSlides.Command.DeleteBannerCmd
{
    public class DeleteBannerHandler : IRequestHandler<DeleteBannerRequest, Result<bool>>
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly ITransectionRepository _transection;
        private readonly IInternoEventPublisher _Publisher;
        private readonly IUserRepository _UserRepo;
        private readonly BannerImageStorage _imageStorage;
        

        public DeleteBannerHandler(IBannerRepository bannerRepository, IUserRepository UserRepo, IInternoEventPublisher Publisher, ITransectionRepository transection, BannerImageStorage imageStorage)
        {
            _bannerRepository = bannerRepository;
            _transection = transection;
            _Publisher = Publisher;
            _UserRepo = UserRepo;
            _imageStorage = imageStorage;
        }

        public async Task<Result<bool>> Handle(DeleteBannerRequest request, CancellationToken cancellationToken)
        {
            var transection = await _transection.BeginTransactionAsync();
            if (transection == null)
            {
                return Result<bool>.InternalError("Failed To Start Transection");
            }
            try
            {
                var existing = await _bannerRepository.GetBannerByID(request.BannerID, false);
                if (existing == null)
                {
                    return Result<bool>.NotFound("Banner Wasnt Found");
                }

                string JsonNewValues = JsonSerializer.Serialize<BannerSlide>(existing);
                existing.IsDeleted = true;

                if (!_imageStorage.DeleteImage(Path.Combine(_imageStorage.CurrentDir, existing.ImageUrl)))
                {
                    return Result<bool>.InternalError("Failed To Delete image");
                }
                if (!await _bannerRepository.SaveChanges())
                {
                    return Result<bool>.InternalError("Failed To SaveChanges");
                }

                string JsonOldValues = JsonSerializer.Serialize<BannerSlide>(existing);
                AuditRequest AuditRequest = new(await _UserRepo.GetAdminID(), ActionTypeEnum.Delete, nameof(BannerSlide), JsonOldValues, JsonNewValues);
                await _Publisher.PublishAuditAsync(AuditRequest);

                await transection.CommitAsync();
                return Result<bool>.Success(existing.IsDeleted);
            }
            catch (Exception)
            {
                await transection.RollbackAsync();
                return Result<bool>.InternalError("SomeThing Went Wrong Check Logs");
            }
        }
    }
}
