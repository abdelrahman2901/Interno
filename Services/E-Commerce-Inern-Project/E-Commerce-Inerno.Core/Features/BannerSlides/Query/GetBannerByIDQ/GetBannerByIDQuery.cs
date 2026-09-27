using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.BannerSlideDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.BannerSlides.Query.GetBannerByIDQ
{
    public record GetBannerByIDQuery(Guid BannerID) : IRequest<Result<BannerSlideResponse>>;
}
