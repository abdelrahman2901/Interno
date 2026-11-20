using AutoMapper;
using E_Commerce_Inerno.Core.Communication;
using E_Commerce_Inerno.Core.RepositoryContracts.ICategoryRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Inerno.Core.Features.Categories.Commands.UpdateCategoryCommand;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CategoryDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;

namespace E_Commerce_Inerno.Core.Features.Categories.Commands.UpdateCategoryCommand
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryRequest, Result<CategoryResponse>>
    {
        private readonly ICategoryRepository _CatRepo;
        private readonly IMapper _Mapper;
        private readonly CategoryImageStorage _imageStorage;
        private readonly IInternoEventPublisher _Publisher;
        private readonly IUserRepository _UserRepo;
        private readonly IDistributedCache _cache;

        public UpdateCategoryHandler(ICategoryRepository catRepo, IDistributedCache cache, IUserRepository UserRepo, IInternoEventPublisher Publisher, CategoryImageStorage imageStorage, IMapper mapper)
        {
            _CatRepo = catRepo;
            _Mapper = mapper;
            _imageStorage = imageStorage;
            _Publisher = Publisher;
            _cache = cache;
            _UserRepo = UserRepo;
        }

        public async Task<Result<CategoryResponse>> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            Category? cat = await _CatRepo.GetCategoryById(request.CategoryID);
            if (cat == null)
            {
                return Result<CategoryResponse>.NotFound("Category Wasnt FOund.");
            }
            string JsonOldValues = GetCategoryJson(cat);

            cat.CategoryName = request.CategoryName ?? cat.CategoryName;
            cat.ParentCategoryID = request.ParentCategoryID ?? cat.ParentCategoryID;
            if (request.CategoryImage != null && cat.HashImage != _imageStorage.HashImage(request.CategoryImage))
            {
                string? path = await _imageStorage.UpdateImage(request.CategoryImage, cat.CategoryImageUrl);
                if (path == null)
                {
                    return Result<CategoryResponse>.InternalError("Failed To Update Category Image");
                }
                cat.CategoryImageUrl = path;
                cat.HashImage = _imageStorage.HashImage(request.CategoryImage);
            }

            if (!await _CatRepo.SaveChanges())
            {
                return Result<CategoryResponse>.InternalError("Failed To Save Chnages");
            }

            string JsonNewValues = GetCategoryJson(cat);
            AuditRequest AuditRequest = new(await _UserRepo.GetAdminID(), ActionTypeEnum.Update, nameof(Category), JsonOldValues, JsonNewValues);
            await _Publisher.PublishAuditAsync(AuditRequest);

            string CatKey = $"Categories";
            await _cache.RemoveAsync(CatKey);
            return Result<CategoryResponse>.Success(_Mapper.Map<CategoryResponse>(cat));
        }
        public string GetCategoryJson(Category cat)
        {
            string metadata = JsonSerializer.Serialize(new
            {
                cat.CategoryID,
                cat.CategoryName,
                cat.ParentCategoryID,
                cat.CategoryImageUrl,
                cat.HashImage
            });
            return metadata;
        }

    }
}
