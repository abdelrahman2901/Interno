using AutoMapper;
using E_Commerce_Inerno.Core.RepositoryContracts.IProductRatesRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ProductRatesDTO;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.ProductRate.Query.GetUserRatingQ
{
    public class GetUserRatingHandler : IRequestHandler<GetUserRatingQuery, Result<IEnumerable<ProductRateResponse>>>
    {
        private readonly IProductRatesRepository _ProductRatesRepo;
        private readonly IMapper _mapper;

        public GetUserRatingHandler(IProductRatesRepository productRatesRepo, IMapper mapper)
        {
            _ProductRatesRepo = productRatesRepo;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ProductRateResponse>>> Handle(GetUserRatingQuery request, CancellationToken cancellationToken)
        {
            var rates = await _ProductRatesRepo.GetUserRating(request.UserID);
            if (rates == null)
            {
                return Result<IEnumerable<ProductRateResponse>>.NotFound("Rate Wasnt Found");
            }

            return Result<IEnumerable<ProductRateResponse>>.Success(rates.Select(rate => _mapper.Map<ProductRateResponse>(rate)));
        }
    }
}
