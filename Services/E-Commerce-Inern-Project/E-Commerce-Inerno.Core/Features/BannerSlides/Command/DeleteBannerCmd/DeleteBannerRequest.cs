using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.BannerSlides.Command.DeleteBannerCmd
{
    public record DeleteBannerRequest(Guid BannerID) : IRequest<Result<bool>>;
}
