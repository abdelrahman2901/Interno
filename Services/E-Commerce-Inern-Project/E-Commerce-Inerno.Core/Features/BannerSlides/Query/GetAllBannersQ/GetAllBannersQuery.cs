using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.BannerSlideDTO;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.BannerSlides.Query.GetAllBannersQ
{
    public record GetAllBannersQuery() : IRequest<Result<IEnumerable<BannerSlideResponse>>>;
}
