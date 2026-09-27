using E_Commerce_Inern_Project.Core.Policies;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.ICategoryRepo;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IProductRepo;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Polly;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.Products.Commands.DeleteProductCommand
{
    public class DeleteProducrHandler : IRequestHandler<DeleteProductCmd, Result<bool>>
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _CategoryRepo;
        private readonly IUserRepository _UserRepo;
        private readonly ProductImageStorage _imageStorage;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly IAsyncPolicy _policy;
        private readonly string AuditKey = "Interno.Audit";

        public DeleteProducrHandler(IProductRepository productRepo, IPollyPolicy AsyncPolicy, IUserRepository UserRepo, ICategoryRepository CategoryRepo, IRabbitMQPublisher Publisher, ProductImageStorage imageStorage)
        {
            _productRepo = productRepo;
            _CategoryRepo = CategoryRepo;
            _UserRepo = UserRepo;
            _Publisher = Publisher;
            _imageStorage = imageStorage;
            _policy = AsyncPolicy.GetPolicy();
        }

        public async Task<Result<bool>> Handle(DeleteProductCmd request, CancellationToken cancellationToken)
        {
            Product? Product = await _productRepo.GetProductByID(request.ProductID, true);
            Category? Category = await _CategoryRepo.GetCategoryById(Product.CategoryID);

            if (Product == null)
            {
                return Result<bool>.NotFound("Product Doesnt Exists");
            }
            string JsonOldValues = JsonSerializer.Serialize<Product>(Product);
            Product.IsDeleted = true;

            bool result = await _productRepo.SaveChanges();
            if (!result)
            {
                return Result<bool>.InternalError("Failed To Save Changes");
            }

            if (!_imageStorage.DeleteImage(Path.Combine(_imageStorage.CategoryDir(Category), Product.ProductImageUrl)))
            {
                return Result<bool>.InternalError("Failed To Delete Image");
            }

            string JsonNewValues = JsonSerializer.Serialize<Product>(Product);
            AuditRequest AuditRequest = new(await _UserRepo.GetAdminID(), ActionTypeEnum.Delete, nameof(Product), JsonOldValues, JsonNewValues);

            await _policy.ExecuteAsync(async () =>
            {
                await _Publisher.Publish(AuditKey, AuditRequest);
            });

            return Result<bool>.Success(result);
        }
    }
}
