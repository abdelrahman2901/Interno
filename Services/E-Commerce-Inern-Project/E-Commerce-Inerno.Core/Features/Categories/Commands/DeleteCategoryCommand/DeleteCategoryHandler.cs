using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.ICategoryRepo;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.Categories.Commands.DeleteCategoryCommand
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCmd, Result<bool>>
    {
        private readonly ICategoryRepository _CatRepo;
        private readonly CategoryImageStorage _imageStorage;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly IUserRepository _UserRepo;
        private readonly IDistributedCache _cache;
        private readonly string _AuditRoutingKey = "Interno.Audit";

        public DeleteCategoryHandler(ICategoryRepository catRepo, IDistributedCache cache, IUserRepository UserRepo, IRabbitMQPublisher Publisher, CategoryImageStorage imageStorage)
        {
            _CatRepo = catRepo;
            _imageStorage = imageStorage;
            _Publisher = Publisher;
            _cache = cache;
            _UserRepo = UserRepo;
        }

        public async Task<Result<bool>> Handle(DeleteCategoryCmd request, CancellationToken cancellationToken)
        {
            Category? Cat = await _CatRepo.GetCategoryByIdIncludingSubCat(request.CatID);

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
                if (!_imageStorage.DeleteImage(Cat.CategoryImageUrl))
                {
                    return Result<bool>.NotFound("Failed To Delte  Category Image");
                }

            string JsonNewValues = JsonSerializer.Serialize<Category>(Cat);
            AuditRequest AuditRequest = new(await _UserRepo.GetAdminID(), ActionTypeEnum.Delete, nameof(Category), JsonOldValues, JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            string CatKey = $"Category:{Cat.CategoryID}";
            await _cache.RemoveAsync(CatKey);

            return Result<bool>.Success(result);
        }
    }
}
