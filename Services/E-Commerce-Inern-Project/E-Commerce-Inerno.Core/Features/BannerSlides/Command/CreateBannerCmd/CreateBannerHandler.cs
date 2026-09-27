using AutoMapper;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ITransectionRepo;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IBannerRepo;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IColorRepo;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.BannerSlides.Command.CreateBannerCmd
{
    public class CreateBannerHandler : IRequestHandler<CreateBannerRequest, Result<bool>>
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IColorRepository _ColorRepo;
        private readonly IMapper _mapper;
        private readonly ITransectionRepository _transection;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly IUserRepository _UserRepo;
        private readonly BannerImageStorage _imageStorage;
        private readonly string _AuditRoutingKey = "Interno.Audit";

        public CreateBannerHandler(IBannerRepository bannerRepository, IUserRepository UserRepo, IRabbitMQPublisher Publisher, ITransectionRepository transection, IColorRepository ColorRepo, BannerImageStorage imageStorage, IMapper mapper)
        {
            _bannerRepository = bannerRepository;
            _transection = transection;
            _ColorRepo = ColorRepo;
            _mapper = mapper;
            _Publisher = Publisher;
            _UserRepo = UserRepo;
            _imageStorage = imageStorage;
        }

        public async Task<Result<bool>> Handle(CreateBannerRequest request, CancellationToken cancellationToken)
        {
            var transection = await _transection.BeginTransactionAsync();
            if (transection == null)
            {
                return Result<bool>.InternalError("Failed To Start Transection");
            }
            try
            {
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
                BannerSlide Banner = _mapper.Map<BannerSlide>(request);
                string? path = await _imageStorage.UploadImage(request.BannerImage);
                if (path == null)
                {
                    return Result<bool>.InternalError("Failed To Upload Banner Image");
                }
                Banner.ImageUrl = path;
                Banner.ImageHash = _imageStorage.HashImage(request.BannerImage);

                var result = await _bannerRepository.AddAsync(Banner);
                if (!result)
                {
                    return Result<bool>.InternalError("Failed To Add Banner");
                }

                if (!await _bannerRepository.SaveChanges())
                {
                    return Result<bool>.InternalError("Failed To SaveChanges");
                }

                string JsonNewValues = JsonSerializer.Serialize<BannerSlide>(Banner);
                AuditRequest AuditRequest = new(await _UserRepo.GetAdminID(), ActionTypeEnum.Delete, nameof(BannerSlide), null, JsonNewValues);
                await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

                await transection.CommitAsync();
                return Result<bool>.Success(result);
            }
            catch (Exception ex)
            {
                await transection.RollbackAsync();
                return Result<bool>.InternalError("SomeThing Went Wrong Check Logs");
            }
        }
    }
}
