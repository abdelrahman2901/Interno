using AutoMapper;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ITransectionRepo;
using E_Commerce_Inern_Project.Core.Features.Products.Commands.UpdateProductRatingCommand;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IProductRatesRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.ProductRate.Command.CreateProductRateCmd
{
    public class CreateProductRateHandler : IRequestHandler<CreateProductRateRequest, Result<bool>>
    {
        private readonly IProductRatesRepository _ProductRatesRepo;
        private readonly ITransectionRepository _transection;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly string _AuditRoutingKey = "Interno.Audit";

        public CreateProductRateHandler(IProductRatesRepository productRatesRepo, IRabbitMQPublisher Publisher, ITransectionRepository transection, IMediator mediator, IMapper mapper)
        {
            _ProductRatesRepo = productRatesRepo;
            _mapper = mapper;
            _Publisher = Publisher;
            _transection = transection;
            _mediator = mediator;
        }

        public async Task<Result<bool>> Handle(CreateProductRateRequest request, CancellationToken cancellationToken)
        {
            var NewRates = request.rates;
            var transection = await _transection.BeginTransactionAsync();
            if (transection == null)
            {
                return Result<bool>.InternalError("Failed To initial Transection");
            }
            try
            {
                foreach (var rate in NewRates)
                {
                    if (await _ProductRatesRepo.isUserHasRatingForProduct(rate))
                    {
                        return Result<bool>.BadRequest("User Already Posted A Rate For That Product");
                    }
                }

                var NewRatesEntities = NewRates.Select(rate => _mapper.Map<ProductRates>(rate)).ToList();
                var result = await _ProductRatesRepo.AddListAsync(NewRatesEntities);
                if (!result)
                {
                    return Result<bool>.InternalError("Failed To Add Rate");
                }

                if (!await _ProductRatesRepo.SaveChanges())
                {
                    return Result<bool>.InternalError("Failed To SaveChanges");
                }

                foreach (var rate in NewRates)
                {
                    UpdateProductRatingRequest UpdateRatingrequest = new(rate.ProductID, rate.Rating);
                    var updateResult = await _mediator.Send(UpdateRatingrequest, cancellationToken);
                    if (!updateResult.IsSuccess)
                    {
                        return Result<bool>.InternalError("SomeThing Went Wrong While Updateing product Rating ,Check Logs");
                    }
                }
                string JsonNewValues = JsonSerializer.Serialize<IEnumerable<ProductRates>>(NewRatesEntities);
                AuditRequest AuditRequest = new(NewRates.FirstOrDefault().UserID, ActionTypeEnum.Create, nameof(ProductRates), null, JsonNewValues);
                await _Publisher.Publish<AuditRequest>(_AuditRoutingKey, AuditRequest);

                await transection.CommitAsync();
                return Result<bool>.Success(result);
            }
            catch (Exception ex)
            {
                await transection.RollbackAsync();
                return Result<bool>.InternalError($"Error: {ex.Message}");
            }
        }
    }
}
