using AutoMapper;
using E_Commerce_Inerno.Core.Communication;
using E_Commerce_Inerno.Core.RepositoryContracts.ICategoryRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CategoryDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;

namespace E_Commerce_Inerno.Core.Features.Categories.Commands.CreateCategoryCommad
{
    public class CreateCategoryCmdHandler : IRequestHandler<CategoryRequest, Result<CategoryResponse>>
    {
        private readonly ICategoryRepository _CatRepo;
        private readonly IMapper _Mapper;
        private readonly CategoryImageStorage _imageStorage;
        private readonly IInternoEventPublisher _Publisher;
        private readonly IUserRepository _UserRepo;
        private readonly IDistributedCache _cache;

        public CreateCategoryCmdHandler(ICategoryRepository catRepo, IDistributedCache cache, IUserRepository UserRepo, IInternoEventPublisher Publisher, CategoryImageStorage imageStorage, IMapper mapper)
        {
            _CatRepo = catRepo;
            _Mapper = mapper;
            _imageStorage = imageStorage;
            _Publisher = Publisher;
            _cache = cache;
            _UserRepo = UserRepo;
        }

        public async Task<Result<CategoryResponse>> Handle(CategoryRequest request, CancellationToken cancellationToken)
        {
            if (await _CatRepo.IsCategoryExistsByName(request.CategoryName))
            {
                return Result<CategoryResponse>.BadRequest("Category Already Exists");
            }
            Category? NewCat = _Mapper.Map<Category>(request);
            NewCat.CategoryID = Guid.NewGuid();

            if (request.CategoryImage != null)
            {
                string? path = await _imageStorage.UploadImage(request.CategoryImage);
                if (path == null)
                {
                    return Result<CategoryResponse>.InternalError("Could Upload Category image");
                }
                NewCat.CategoryImageUrl = path;
                NewCat.HashImage = _imageStorage.HashImage(request.CategoryImage);
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
            AuditRequest AuditRequest = new(await _UserRepo.GetAdminID(), ActionTypeEnum.Create, nameof(Category), null, JsonNewValues);
            await _Publisher.PublishAuditAsync(AuditRequest);

            // Cache the newly created category
            string CatKey = "Categories";
            await _cache.RemoveAsync(CatKey);

            return Result<CategoryResponse>.Success(_Mapper.Map<CategoryResponse>(NewCat));
        }
    }
}
