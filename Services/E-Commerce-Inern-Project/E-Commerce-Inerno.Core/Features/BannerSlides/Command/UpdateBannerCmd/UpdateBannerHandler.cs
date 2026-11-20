using AutoMapper;
using E_Commerce_Inerno.Core.Domain.RepositoryContracts.ITransectionRepo;
using E_Commerce_Inerno.Core.Communication;

using E_Commerce_Inerno.Core.RepositoryContracts.IBannerRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IColorRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;

namespace E_Commerce_Inerno.Core.Features.BannerSlides.Command.UpdateBannerCmd
{
    public class UpdateBannerHandler : IRequestHandler<UpdateBannerRequest, Result<bool>>
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IMapper _mapper;
        private readonly IColorRepository _ColorRepo;
        private readonly ITransectionRepository _transection;
        private readonly IInternoEventPublisher _Publisher;
        private readonly IUserRepository _UserRepo;
        private readonly BannerImageStorage _imageStorage;
        

        public UpdateBannerHandler(IBannerRepository bannerRepository, IUserRepository UserRepo, IInternoEventPublisher Publisher, ITransectionRepository transection, IColorRepository ColorRepo, BannerImageStorage imageStorage, IMapper mapper)
        {
            _bannerRepository = bannerRepository;
            _transection = transection;
            _ColorRepo = ColorRepo;
            _mapper = mapper;
            _Publisher = Publisher;
            _UserRepo = UserRepo;
            _imageStorage = imageStorage;
        }

        public async Task<Result<bool>> Handle(UpdateBannerRequest request, CancellationToken cancellationToken)
        {
            var transection = await _transection.BeginTransactionAsync();
            if (transection == null)
            {
                return Result<bool>.InternalError("Failed To Start Transection");
            }
            try
            {
                var existing = await _bannerRepository.GetBannerByID(request.BannerSlideID, true);
                if (existing == null)
                {
                    return Result<bool>.NotFound("Banner Wasnt Found");
                }

                string JsonOldValues = JsonSerializer.Serialize<BannerSlide>(existing);

                if (request.AccentColorID == null || request.BackgroundColorID == null)
                {
                    if (request.AccentColorID == null)
                    {

                        Colors NewColor = new()
                        {
                            ColorID = Guid.NewGuid(),
                            ColorName = null,
                            IsDeleted = false,
                            ColorHexCode = request.AccentColorHex
                        };
                        request.AccentColorID = NewColor.ColorID;
                        await _ColorRepo.CreateColors(NewColor);
                    }
                    if (request.BackgroundColorID == null)
                    {

                        Colors NewColor = new()
                        {
                            ColorID = Guid.NewGuid(),
                            ColorName = null,
                            IsDeleted = false,
                            ColorHexCode = request.BackgroundColorHex
                        };
                        request.BackgroundColorID = NewColor.ColorID;
                        await _ColorRepo.CreateColors(NewColor);
                    }
                    await _ColorRepo.SaveChanges();
                }

                _mapper.Map(request, existing);

                if (request.BannerImage != null)
                {
                    string? NewPath = await _imageStorage.UpdateImage(request.BannerImage, existing.ImageUrl);
                    if (NewPath == null)
                    {
                        return Result<bool>.InternalError("Failed To Update Image");
                    }
                    existing.ImageUrl = NewPath;
                    existing.ImageHash = _imageStorage.HashImage(request.BannerImage);
                }

                if (!await _bannerRepository.SaveChanges())
                {
                    return Result<bool>.InternalError("Failed To SaveChanges");
                }
                string JsonNewValues = JsonSerializer.Serialize<BannerSlide>(existing);

                AuditRequest AuditRequest = new(await _UserRepo.GetAdminID(), ActionTypeEnum.Update, nameof(BannerSlide), JsonOldValues, JsonNewValues);
                await _Publisher.PublishAuditAsync(AuditRequest);

                await transection.CommitAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception)
            {
                await transection.RollbackAsync();
                return Result<bool>.InternalError("Failed To SaveChanges");
            }
        }
    }
}
