using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.BannerSlideDTO;
using MediatR;


namespace E_Commerce_Inern_Project.Core.Features.BannerSlide.Query.GetAllBannersQ
{
    public record GetAllBannersQuery() : IRequest<Result<IEnumerable<BannerSlideResponse>>>;
}
