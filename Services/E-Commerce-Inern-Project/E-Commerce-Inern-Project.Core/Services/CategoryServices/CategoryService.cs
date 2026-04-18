using AutoMapper;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICategoryRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IUserRepo;
using E_Commerce_Inern_Project.Core.DTO.CategoryDTO;
using E_Commerce_Inern_Project.Core.Features.Category.Commands.CreateCategoryCommad;
using E_Commerce_Inern_Project.Core.Features.Category.Commands.UpdateCategoryCommand;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICategoryServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _CatRepo;
        private readonly IMapper _Mapper;
        private readonly ILogger<CategoryService> _logger;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly IUserRepository _UserRepo;
        private readonly IDistributedCache _cache;
        private readonly string _CurrentDir = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\CategoryImages");
        private readonly string _AuditRoutingKey = "Interno.Audit";
        public CategoryService(ICategoryRepository catRepo, IDistributedCache cache, IUserRepository UserRepo, IRabbitMQPublisher Publisher, IMapper mapper, ILogger<CategoryService> logger)
        {
            _CatRepo = catRepo;
            _Mapper = mapper;
            _logger = logger;
            _Publisher = Publisher;
            _cache = cache;
            _UserRepo = UserRepo;
        }

        public async Task<Result<CategoryResponse>> CreateCategory(CategoryRequest Request)
        {
            if (await _CatRepo.IsCategoryExistsByName(Request.CategoryName))
            {
                return Result<CategoryResponse>.BadRequest("Category Already Exists");
            }
            Category? NewCat = _Mapper.Map<Category>(Request);
            NewCat.CategoryID = Guid.NewGuid();

            if (Request.CategoryImage != null)
            {
                string? path = await UploadImage(Request.CategoryImage);
                if (path == null)
                {
                    return Result<CategoryResponse>.InternalError("Could Upload Category image");
                }
                NewCat.CategoryImageUrl = path;
                NewCat.HashImage = HashImage(Request.CategoryImage);
            }

            if (_CatRepo.AddCategory(NewCat) == null)
            {
                return Result<CategoryResponse>.InternalError("Failed To add Category");
            }
            if (!await _CatRepo.SaveChanges())
            {
                return Result<CategoryResponse>.InternalError("Failed To Save Changes");
            }
            string JsonNewValues = JsonSerializer.Serialize<Category>(NewCat);  
            AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Create, nameof(Category), null, JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);


            // Cache the newly created category
            string CatKey = $"Category:{NewCat.CategoryID}";
            string CatJson = JsonSerializer.Serialize<CategoryResponse>(_Mapper.Map<CategoryResponse>(NewCat));
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)).SetAbsoluteExpiration(TimeSpan.FromHours(1));
            await _cache.SetStringAsync(CatKey, CatJson, options);

            return Result<CategoryResponse>.Success(_Mapper.Map<CategoryResponse>(NewCat));
        }
        public async Task<Result<bool>> DeleteCategory(Guid id)
        {
            Category? Cat = await _CatRepo.GetCategoryByIdIncludingSubCat(id);

            if (Cat == null)
            {
                return Result<bool>.NotFound("Category Deosnt Exists");
            }

            string JsonOldValues = JsonSerializer.Serialize<Category>(Cat);
            Cat.IsDeleted = true;

            if (Cat.SubCategories.Any())
            {
                foreach (var subcat in Cat.SubCategories)
                {
                    subcat.IsDeleted = true;
                }
            }

            bool result = await _CatRepo.SaveChanges();
            if (!result)
            {
                return Result<bool>.InternalError("Failed To Save Changes");
            }
            if (!string.IsNullOrEmpty(Cat.CategoryImageUrl))
                if (!DeleteImage(Cat.CategoryImageUrl))
                {
                    return Result<bool>.NotFound("Failed To Delte  Category Image");
                }

            string JsonNewValues = JsonSerializer.Serialize<Category>(Cat);
            AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Delete, nameof(Category),JsonOldValues,JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            string CatKey = $"Category:{Cat.CategoryID}";
            await _cache.RemoveAsync(CatKey);

            return Result<bool>.Success(result);
        }

        public async Task<Result<CategoryResponse>> UpdateCategory(UpdateCategoryRequest UpdateReq)
        {
            Category? cat = await _CatRepo.GetCategoryById(UpdateReq.CategoryID);
            if (cat == null)
            {
                return Result<CategoryResponse>.NotFound("Category Wasnt FOund.");
            }
            string JsonOldValues = JsonSerializer.Serialize<Category>(cat);


            cat.CategoryName = UpdateReq.CategoryName ?? cat.CategoryName;
            cat.ParentCategoryID = UpdateReq.ParentCategoryID ?? cat.ParentCategoryID;
            if (UpdateReq.CategoryImage != null && cat.HashImage != HashImage(UpdateReq.CategoryImage))
            {
                string? path = await UpdateImage(UpdateReq.CategoryImage, cat.CategoryImageUrl);
                if (path == null)
                {
                    return Result<CategoryResponse>.InternalError("Failed To Update Category Image");
                }
                cat.CategoryImageUrl = path;
                cat.HashImage = HashImage(UpdateReq.CategoryImage);
            }


            if (!await _CatRepo.SaveChanges())
            {
                return Result<CategoryResponse>.InternalError("Failed To Save Chnages");

            }
            
            string JsonNewValues = JsonSerializer.Serialize<Category>(cat);
            AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Update, nameof(Category), JsonOldValues, JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);


            string CatKey = $"Category:{cat.CategoryID}";
            await _cache.RemoveAsync(CatKey);
            return Result<CategoryResponse>.Success(_Mapper.Map<CategoryResponse>(cat));

        }

       
        public async Task<Result<IEnumerable<SubCategoryResponse>>> GetCategoriesWithSubCat()
        {

            string CatsKey = "Categories";
            string? cachedCats = await _cache.GetStringAsync(CatsKey);
            if (cachedCats != null)
            {
                var cachedCategories = JsonSerializer.Deserialize<IEnumerable<SubCategoryResponse>>(cachedCats);
                return Result<IEnumerable<SubCategoryResponse>>.Success(cachedCategories);
            }


            var Categories = await _CatRepo.GetCategoriesWithSubCat();
            if (!Categories.Any())
            {
                return Result<IEnumerable<SubCategoryResponse>>.NotFound("No Categories Was Found");
            }

            string CatsJson = JsonSerializer.Serialize(Categories.Select(s => _Mapper.Map<SubCategoryResponse>(s)));
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)).SetAbsoluteExpiration(TimeSpan.FromHours(1));

            await _cache.SetStringAsync(CatsKey, CatsJson, options);

            return Result<IEnumerable<SubCategoryResponse>>.Success(Categories.Select(s => _Mapper.Map<SubCategoryResponse>(s)));
        }

        public async Task<Result<CategoryResponse?>> GetCategoryByID(Guid CatID)
        {
            string CatKey = $"Category:{CatID}";
            string? CachedCat = await _cache.GetStringAsync(CatKey);
            if(CachedCat != null)
            {
                var cachedCategory = JsonSerializer.Deserialize<CategoryResponse>(CachedCat);
                return Result<CategoryResponse?>.Success(cachedCategory);
            }

            var Category = await _CatRepo.GetCategoryById(CatID);
            if (Category == null)
            {
                return Result<CategoryResponse?>.NotFound("No Categories Was Found");
            }

            var response = _Mapper.Map<CategoryResponse>(Category);

            string CatJson = JsonSerializer.Serialize<CategoryResponse>(_Mapper.Map<CategoryResponse>(Category));
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)).SetAbsoluteExpiration(TimeSpan.FromHours(1));
            await _cache.SetStringAsync(CatKey, CatJson, options);

            return Result<CategoryResponse?>.Success(response);
        }

        private async Task<string?> UploadImage(IFormFile image)
        {
            if (!Directory.Exists(_CurrentDir))
            {
                Directory.CreateDirectory(_CurrentDir);
            }

            string imagename = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            try
            {
                using (FileStream stream = new(Path.Combine(_CurrentDir, imagename), FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                return imagename;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed To Upload Image");

                return null;
            }
        }

        private async Task<string?> UpdateImage(IFormFile image, string oldImage)
        {
            string ImageDir = Path.Combine(_CurrentDir, oldImage);
            if (!DeleteImage(ImageDir))
            {
                return null;
            }
            return await UploadImage(image);
        }

        private bool DeleteImage(string image)
        {
            string path = Path.Combine(_CurrentDir, image);
            if (!File.Exists(path))
            {
                _logger.LogError("Image Doesnt Exists");
                return false;
            }
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed To Delete Old Image exception :{Message}", ex.Message);

                if (ex.InnerException != null)
                {
                    _logger.LogError("Failed To Delete Old Image Innerexception :{Message}", ex.InnerException.Message);
                }

                return false;
            }
        }
        private string HashImage(IFormFile image)
        {
            using (var sha256 = SHA256.Create())
            using (Stream stream = image.OpenReadStream())
            {
                byte[] bytes = sha256.ComputeHash(stream);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        private async Task<Guid> GetAdminID()
        {
            var admin = await _UserRepo.GetApplicationUserByEmail("Admin@gmail.com");

            return admin.Id;
        }
    }
}
