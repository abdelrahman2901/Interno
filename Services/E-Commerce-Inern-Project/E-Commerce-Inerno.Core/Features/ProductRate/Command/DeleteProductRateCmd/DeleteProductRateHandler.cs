using E_Commerce_Inerno.Core.Communication;

using E_Commerce_Inerno.Core.RepositoryContracts.IProductRatesRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;

namespace E_Commerce_Inerno.Core.Features.ProductRate.Command.DeleteProductRateCmd
{
    public class DeleteProductRateHandler : IRequestHandler<DeleteProductRateRequest, Result<bool>>
    {
        private readonly IProductRatesRepository _ProductRatesRepo;
        private readonly IInternoEventPublisher _Publisher;
        

        public DeleteProductRateHandler(IProductRatesRepository productRatesRepo, IInternoEventPublisher Publisher)
        {
            _ProductRatesRepo = productRatesRepo;
            _Publisher = Publisher;
        }

        public async Task<Result<bool>> Handle(DeleteProductRateRequest request, CancellationToken cancellationToken)
        {
            var rate = await _ProductRatesRepo.GetProductRateByID(request.RateID, true);
            if (rate == null)
            {
                return Result<bool>.NotFound("Rate Wasnt Found");
            }
            string JsonOldValues = JsonSerializer.Serialize<ProductRates>(rate);
            rate.IsDeleted = true;
            if (!await _ProductRatesRepo.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaveChanges");
            }
            string JsonNewValues = JsonSerializer.Serialize<ProductRates>(rate);
            AuditRequest AuditRequest = new(rate.UserID, ActionTypeEnum.Delete, nameof(ProductRates), JsonOldValues, JsonNewValues);
            await _Publisher.PublishAuditAsync(AuditRequest);
            return Result<bool>.Success(rate.IsDeleted);
        }
    }
}
