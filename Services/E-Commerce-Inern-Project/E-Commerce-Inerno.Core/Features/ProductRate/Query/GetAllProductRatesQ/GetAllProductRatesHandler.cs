using AutoMapper;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IProductRatesRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ProductRatesDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.ProductRate.Query.GetAllProductRatesQ
{
    public class GetAllProductRatesHandler : IRequestHandler<GetAllProductRatesQuery, Result<IEnumerable<ProductRateResponse>>>
    {
        private readonly IProductRatesRepository _ProductRatesRepo;
        private readonly IMapper _mapper;

        public GetAllProductRatesHandler(IProductRatesRepository productRatesRepo, IMapper mapper)
        {
            _ProductRatesRepo = productRatesRepo;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ProductRateResponse>>> Handle(GetAllProductRatesQuery request, CancellationToken cancellationToken)
        {
            var rates = await _ProductRatesRepo.GetAllProductRates();
            if (rates == null)
            {
                return Result<IEnumerable<ProductRateResponse>>.NotFound("Rate Wasnt Found");
            }

            return Result<IEnumerable<ProductRateResponse>>.Success(rates.Select(rate => _mapper.Map<ProductRateResponse>(rate)));
        }
    }
}
