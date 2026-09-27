using E_Commerce_Inern_Project.Core.Policies;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IProductRatesRepo;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IProductRepo;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Polly;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.Products.Commands.UpdateProductRatingCommand
{
    public class UpdateProductRatingHandler : IRequestHandler<UpdateProductRatingRequest, Result<bool>>
    {
        private readonly IProductRepository _productRepo;
        private readonly IProductRatesRepository _ProductRatesRepo;
        private readonly IUserRepository _UserRepo;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly IAsyncPolicy _policy;
        private readonly string AuditKey = "Interno.Audit";

        public UpdateProductRatingHandler(IProductRepository productRepo, IPollyPolicy AsyncPolicy, IUserRepository UserRepo, IRabbitMQPublisher Publisher, IProductRatesRepository ProductRatesRepo)
        {
            _productRepo = productRepo;
            _ProductRatesRepo = ProductRatesRepo;
            _UserRepo = UserRepo;
            _Publisher = Publisher;
            _policy = AsyncPolicy.GetPolicy();
        }

        public async Task<Result<bool>> Handle(UpdateProductRatingRequest request, CancellationToken cancellationToken)
        {
            Product? existing = await _productRepo.GetProductByID(request.ProductID, true);
            if (existing == null)
            {
                return Result<bool>.NotFound("Product Doesnt Exists");
            }
            string JsonOldValues = JsonSerializer.Serialize<Product>(existing);

            var ProductRates = await _ProductRatesRepo.GetAllProductRatesForProduct(existing.ProductID);
            double avgRating = ProductRates.Any() ? ProductRates.Average(r => r.Rating) : 0;

            existing.Rating = existing.TotalRating == 0 ? request.Rating : CalculateRatingCls.CalcuateTheRating(existing.TotalRating, avgRating);
            existing.TotalRating += 1;

            if (!await _productRepo.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaveChanges");
            }

            string JsonNewValues = JsonSerializer.Serialize<Product>(existing);
            AuditRequest AuditRequest = new(await _UserRepo.GetAdminID(), ActionTypeEnum.Update, nameof(Product), JsonOldValues, JsonNewValues);

            await _policy.ExecuteAsync(async () =>
            {
                await _Publisher.Publish(AuditKey, AuditRequest);
            });

            return Result<bool>.Success(existing.IsDeleted);
        }
    }
}
