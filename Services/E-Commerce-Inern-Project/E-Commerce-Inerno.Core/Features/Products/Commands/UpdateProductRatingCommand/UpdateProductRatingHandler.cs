using E_Commerce_Inerno.Core.Policies;
using E_Commerce_Inerno.Core.Communication;
using E_Commerce_Inerno.Core.RepositoryContracts.IProductRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Polly;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;

namespace E_Commerce_Inerno.Core.Features.Products.Commands.UpdateProductRatingCommand
{
    public class UpdateProductRatingHandler : IRequestHandler<UpdateProductRatingRequest, Result<bool>>
    {
        private readonly IProductRepository _productRepo;
        private readonly IUserRepository _UserRepo;
        private readonly IInternoEventPublisher _Publisher;
        private readonly IAsyncPolicy _policy;

        public UpdateProductRatingHandler(IProductRepository productRepo, IPollyPolicy AsyncPolicy, IUserRepository UserRepo, IInternoEventPublisher Publisher)
        {
            _productRepo = productRepo;
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

            existing.TotalRating += 1;

            if (!await _productRepo.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaveChanges");
            }

            string JsonNewValues = JsonSerializer.Serialize<Product>(existing);
            AuditRequest AuditRequest = new(await _UserRepo.GetAdminID(), ActionTypeEnum.Update, nameof(Product), JsonOldValues, JsonNewValues);

            await _policy.ExecuteAsync(async () =>
            {
                await _Publisher.PublishAuditAsync(AuditRequest);
            });

            return Result<bool>.Success(existing.IsDeleted);
        }
    }
}
