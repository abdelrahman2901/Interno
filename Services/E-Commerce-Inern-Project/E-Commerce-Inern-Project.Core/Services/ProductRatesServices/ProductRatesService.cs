using AutoMapper;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IProductRatesRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ITransectionRepo;
using E_Commerce_Inern_Project.Core.DTO.ProductRatesDTO;
using E_Commerce_Inern_Project.Core.Features.Product.Commands.UpdateProductRatingCommand;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.ServicesContracts.IProductRatesServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.IProductServices;
using MediatR;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Services.ProductRatesServices
{
    public class ProductRatesService : IProductRatesService
    {
        private readonly IProductRatesRepository _ProductRatesRepo;
        private readonly IProductService _ProductService;
        private readonly ITransectionRepository _transection;
        private readonly IMapper _mapper;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly string _AuditRoutingKey = "Interno.Audit";
        public ProductRatesService(IProductRatesRepository productRatesRepo, IRabbitMQPublisher Publisher, ITransectionRepository transection, IProductService ProductService, IMapper mapper)
        {
            _ProductRatesRepo = productRatesRepo;
            _mapper = mapper;
            _Publisher = Publisher;
            _transection=transection;
            _ProductService=ProductService;
        }

        public async Task<Result<bool>> CreateProductRateList(IEnumerable<ProductRateRequest> NewRates)
        {
            var transection =await  _transection.BeginTransactionAsync();
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
                    var updateResult = await _ProductService.UpdateProductRating(UpdateRatingrequest);
                    if (!updateResult.IsSuccess)
                    {

                        return Result<bool>.InternalError("SomeThing Went Wrong While Updateing product Rating ,Check Logs");
                    }
                }
                string JsonNewValues = JsonSerializer.Serialize<IEnumerable<ProductRates>>(NewRatesEntities);
                AuditRequest AuditRequest = new(NewRates.FirstOrDefault().UserID, ActionTypeEnum.Create, nameof(ProductRates),null,JsonNewValues);
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

        public async Task<Result<bool>> DeleteProductRate(Guid RateID)
        {
            var rate = await _ProductRatesRepo.GetProductRateByID_Traking(RateID);
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

        public async Task<Result<IEnumerable<ProductRateResponse>>> GetAllProductRates()
        {
            var rates = await _ProductRatesRepo.GetAllProductRates();
            if (rates == null)
            {
                return Result<IEnumerable<ProductRateResponse>>.NotFound("Rate Wasnt Found");
            }

            return Result<IEnumerable<ProductRateResponse>>.Success(rates.Select(rate => _mapper.Map<ProductRateResponse>(rate)));
        }

        public async Task<Result<ProductRateResponse>> GetProductRate(Guid RateID)
        {
            var rate = await _ProductRatesRepo.GetProductRateByID_NoTracking(RateID);
            if (rate == null)
            {
                return Result<ProductRateResponse>.NotFound("Rate Wasnt Found");
            }

            return Result<ProductRateResponse>.Success(_mapper.Map<ProductRateResponse>(rate));
        }

        public async Task<Result<IEnumerable<ProductRateResponse>>> GetProductRatesForProduct(Guid ProductID)
        {
            var rates = await _ProductRatesRepo.GetAllProductRatesForProduct(ProductID);
            if (rates == null)
            {
                return Result<IEnumerable<ProductRateResponse>>.NotFound("Rate Wasnt Found");
            }

            return Result<IEnumerable<ProductRateResponse>>.Success(rates.Select(rate => _mapper.Map<ProductRateResponse>(rate)));
        }

        public async Task<Result<IEnumerable<ProductRateResponse>>> GetUserRating(Guid UserID)
        {
            var rates = await _ProductRatesRepo.GetUserRating(UserID);
            if (rates == null)
            {
                return Result<IEnumerable<ProductRateResponse>>.NotFound("Rate Wasnt Found");
            }

            return Result<IEnumerable<ProductRateResponse>>.Success(rates.Select(rate => _mapper.Map<ProductRateResponse>(rate)));
        }
    }
}
