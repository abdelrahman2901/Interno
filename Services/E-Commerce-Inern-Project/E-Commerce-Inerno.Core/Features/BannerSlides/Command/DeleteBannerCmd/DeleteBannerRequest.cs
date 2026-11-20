using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.BannerSlides.Command.DeleteBannerCmd
{
    public record DeleteBannerRequest(Guid BannerID) : IRequest<Result<bool>>;
}
