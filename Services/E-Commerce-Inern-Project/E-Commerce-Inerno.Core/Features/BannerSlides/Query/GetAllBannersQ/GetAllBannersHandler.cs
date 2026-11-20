using AutoMapper;
using E_Commerce_Inerno.Core.RepositoryContracts.IBannerRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.BannerSlideDTO;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.BannerSlides.Query.GetAllBannersQ
{
    public class GetAllBannersHandler : IRequestHandler<GetAllBannersQuery, Result<IEnumerable<BannerSlideResponse>>>
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IMapper _mapper;

        public GetAllBannersHandler(IBannerRepository bannerRepository, IMapper mapper)
        {
            _bannerRepository = bannerRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<BannerSlideResponse>>> Handle(GetAllBannersQuery request, CancellationToken cancellationToken)
        {
            var result = await _bannerRepository.GetAllBanners();
            if (!result.Any())
            {
                return Result<IEnumerable<BannerSlideResponse>>.NotFound("No Banner Was Found.");
            }

            return Result<IEnumerable<BannerSlideResponse>>.Success(result.Select(s => _mapper.Map<BannerSlideResponse>(s)));
        }
    }
}
