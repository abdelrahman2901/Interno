using AutoMapper;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IBannerRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.BannerSlideDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.BannerSlides.Query.GetBannerByIDQ
{
    public class GetBannerByIDHandler : IRequestHandler<GetBannerByIDQuery, Result<BannerSlideResponse>>
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IMapper _mapper;

        public GetBannerByIDHandler(IBannerRepository bannerRepository, IMapper mapper)
        {
            _bannerRepository = bannerRepository;
            _mapper = mapper;
        }

        public async Task<Result<BannerSlideResponse>> Handle(GetBannerByIDQuery request, CancellationToken cancellationToken)
        {
            var result = await _bannerRepository.GetBannerByID(request.BannerID, false);
            if (result == null)
            {
                return Result<BannerSlideResponse>.NotFound("Banner Wasnt Found");
            }

            return Result<BannerSlideResponse>.Success(_mapper.Map<BannerSlideResponse>(result));
        }
    }
}
