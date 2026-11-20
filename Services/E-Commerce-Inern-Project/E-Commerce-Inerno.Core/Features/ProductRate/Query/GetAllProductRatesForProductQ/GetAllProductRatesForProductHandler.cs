using AutoMapper;
using E_Commerce_Inerno.Core.RepositoryContracts.IProductRatesRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ProductRatesDTO;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.ProductRate.Query.GetAllProductRatesForProductQ
{
    public class GetAllProductRatesForProductHandler : IRequestHandler<GetAllProductRatesForProductQuery, Result<IEnumerable<ProductRateResponse>>>
    {
        private readonly IProductRatesRepository _ProductRatesRepo;
        private readonly IMapper _mapper;

        public GetAllProductRatesForProductHandler(IProductRatesRepository productRatesRepo, IMapper mapper)
        {
            _ProductRatesRepo = productRatesRepo;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ProductRateResponse>>> Handle(GetAllProductRatesForProductQuery request, CancellationToken cancellationToken)
        {
            var rates = await _ProductRatesRepo.GetAllProductRatesForProduct(request.ProductID);
            if (rates == null)
            {
                return Result<IEnumerable<ProductRateResponse>>.NotFound("Rate Wasnt Found");
            }

            return Result<IEnumerable<ProductRateResponse>>.Success(rates.Select(rate => _mapper.Map<ProductRateResponse>(rate)));
        }
    }
}
