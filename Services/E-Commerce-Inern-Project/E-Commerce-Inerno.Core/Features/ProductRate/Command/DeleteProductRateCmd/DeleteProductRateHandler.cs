using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IProductRatesRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.ProductRate.Command.DeleteProductRateCmd
{
    public class DeleteProductRateHandler : IRequestHandler<DeleteProductRateRequest, Result<bool>>
    {
        private readonly IProductRatesRepository _ProductRatesRepo;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly string _AuditRoutingKey = "Interno.Audit";

        public DeleteProductRateHandler(IProductRatesRepository productRatesRepo, IRabbitMQPublisher Publisher)
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
            await _Publisher.Publish<AuditRequest>(_AuditRoutingKey, AuditRequest);
            return Result<bool>.Success(rate.IsDeleted);
        }
    }
}
