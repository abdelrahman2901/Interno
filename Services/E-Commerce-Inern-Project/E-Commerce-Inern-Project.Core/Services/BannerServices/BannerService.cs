using AutoMapper;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IBannerRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ITransectionRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IUserRepo;
using E_Commerce_Inern_Project.Core.DTO.BannerSlideDTO;
using E_Commerce_Inern_Project.Core.Features.BannerSlide.Command.CreateBannerCmd;
using E_Commerce_Inern_Project.Core.Features.BannerSlide.Command.UpdateBannerCmd;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.ServicesContracts.IBannerServices;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;


namespace E_Commerce_Inern_Project.Core.Services.BannerServices
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IMapper _mapper;
        private readonly ITransectionRepository _transection;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly IUserRepository _UserRepo;

        private readonly string _AuditRoutingKey = "Interno.Audit";
        private readonly string _CurrentDIr =  Path.Combine(Directory.GetCurrentDirectory() ,@"wwwroot\BannerImages"); 
        public BannerService(IBannerRepository bannerRepository, IUserRepository UserRepo, IRabbitMQPublisher Publisher, ITransectionRepository transection, IMapper mapper)
        {
            _bannerRepository = bannerRepository;
            _transection = transection;
            _mapper = mapper;
            _Publisher = Publisher;
            _UserRepo = UserRepo;
        }

        public async Task<Result<bool>> CreateBanner(CreateBannerRequest request)
        {
            var transection = await _transection.BeginTransactionAsync();
            if (transection == null)
            {
                return Result<bool>.InternalError("Failed To Start Transection");
            }
            try
            {
                BannerSlide Banner = _mapper.Map<BannerSlide>(request);

                string? path = await UploadImage(request.BannerImage);
                if (path == null)
                {
                    return Result<bool>.InternalError("Failed To Upload Banner Image");
                }
                Banner.ImageUrl = path;
                Banner.ImageHash = HashImage(request.BannerImage);

                var result = await _bannerRepository.AddAsync(Banner);
                if (!result)
                {
                    return Result<bool>.InternalError("Failed To Add Banner");
                }

                if (!await _bannerRepository.SaveChanges())
                {
                    return Result<bool>.InternalError("Failed To SaveChanges");
                }


                string JsonNewValues=JsonSerializer.Serialize<BannerSlide>(Banner);
                AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Delete, nameof(BannerSlide),null,JsonNewValues);
                await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

                await transection.CommitAsync();
                return Result<bool>.Success(result);
            }
            catch (Exception)
            {

                await transection.RollbackAsync();
                return Result<bool>.InternalError("SomeThing Went Wrong Check Logs");
            }
        }

        public async Task<Result<bool>> DeleteBanner(Guid BannerID)
        {
            var transection = await _transection.BeginTransactionAsync();
            if (transection == null)
            {
                return Result<bool>.InternalError("Failed To Start Transection");
            }
            try
            {

                var existing = await _bannerRepository.GetBannerByID_Traking(BannerID);
                if (existing == null)
                {
                    return Result<bool>.NotFound("Banner Wasnt Found");
                }
                string JsonNewValues = JsonSerializer.Serialize<BannerSlide>(existing);
                existing.IsDeleted = true;
                if (!DeleteImage(Path.Combine(_CurrentDIr, existing.ImageUrl)))
                {
                    return Result<bool>.InternalError("Failed To Delete image");
                }
                if (!await _bannerRepository.SaveChanges())
                {
                    return Result<bool>.InternalError("Failed To SaveChanges");
                }

                string JsonOldValues = JsonSerializer.Serialize<BannerSlide>(existing);
                AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Delete, nameof(BannerSlide),JsonOldValues,JsonNewValues);
                await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

                await transection.CommitAsync();
                return Result<bool>.Success(existing.IsDeleted);
            }
            catch (Exception)
            {
                await transection.RollbackAsync();
                return Result<bool>.InternalError("SomeThing Went Wrong Check Logs");
            }
        }

        public async Task<Result<BannerSlideResponse>> GetBannerSlideByID(Guid BannerID)
        {
            var result = await _bannerRepository.GetBannerByID_NoTracking(BannerID);
            if (result == null)
            {
                return Result<BannerSlideResponse>.NotFound("Banner Wasnt Found");
            }

            return Result<BannerSlideResponse>.Success(_mapper.Map<BannerSlideResponse>(result));
        }

        public async Task<Result<IEnumerable<BannerSlideResponse>>> GetBanners()
        {
            var result = await _bannerRepository.GetAllBanners();
            if (!result.Any())
            {
                return Result<IEnumerable<BannerSlideResponse>>.NotFound("No Banner Was Found.");
            }

            return Result<IEnumerable<BannerSlideResponse>>.Success(result.Select(s => _mapper.Map<BannerSlideResponse>(s)));
        }

        public async Task<Result<bool>> ToggleBannerActiviation(Guid BannerID)
        {
            var existing = await _bannerRepository.GetBannerByID_Traking(BannerID);
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

            AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Update, nameof(BannerSlide),JsonOldValues,JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> UpdateBanner(UpdateBannerRequest request)
        {
            var transection = await _transection.BeginTransactionAsync();
            if(transection == null)
            {
                return Result<bool>.InternalError("Failed To Start Transection");
            }
            try
            {

                var existing = await _bannerRepository.GetBannerByID_Traking(request.BannerSlideID);
                if (existing == null)
                {
                    return Result<bool>.NotFound("Banner Wasnt Found");
                }

                string JsonOldValues = JsonSerializer.Serialize<BannerSlide>(existing);

                _mapper.Map(request, existing);

                if (request.BannerImage != null)
                {
                    string? NewPath = await UpdateImage(request.BannerImage, existing.ImageUrl);
                    if (NewPath == null)
                    {
                        return Result<bool>.InternalError("Failed To Update Image");

                    }
                    existing.ImageUrl = NewPath;
                    existing.ImageHash = HashImage(request.BannerImage);
                }

                if (!await _bannerRepository.SaveChanges())
                {
                    return Result<bool>.InternalError("Failed To SaveChanges");
                }
                string JsonNewValues = JsonSerializer.Serialize<BannerSlide>(existing);

                AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Update, nameof(BannerSlide),JsonOldValues,JsonNewValues);
                await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

                await transection.CommitAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception)
            {
                await transection.RollbackAsync();
                return Result<bool>.InternalError("Failed To SaveChanges");

            }
        }

        private async Task<string?> UploadImage(IFormFile image)
        {

            if (!Directory.Exists(_CurrentDIr))  
            {
                Directory.CreateDirectory(_CurrentDIr);
            }

            string imagename = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            try
            {
                using (FileStream stream = new(Path.Combine(_CurrentDIr, imagename), FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                return imagename;
            }
            catch (Exception)
            {

                return null;
            }
        }
        private async Task<string?> UpdateImage(IFormFile image, string oldImage)
        {
            string ImageDir = Path.Combine(_CurrentDIr, oldImage);
            if (!DeleteImage(ImageDir))
            {
                return null;
            }
            return await UploadImage(image);
        }
        private static bool DeleteImage(string Path)
        {
            if (!File.Exists(Path))
            {
                return false;
            }
            try
            {
                File.Delete(Path);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        private static string HashImage(IFormFile image)
        {
            using var sha256 = SHA256.Create();
            using Stream stream = image.OpenReadStream();
            byte[] hashBytes = sha256.ComputeHash(stream);
            return Convert.ToHexStringLower(hashBytes);
        }

        private async Task<Guid> GetAdminID()
        {
                var admin = await _UserRepo.GetApplicationUserByEmail("Admin@gmail.com");
            return admin.Id;
        }
  
    }
}
