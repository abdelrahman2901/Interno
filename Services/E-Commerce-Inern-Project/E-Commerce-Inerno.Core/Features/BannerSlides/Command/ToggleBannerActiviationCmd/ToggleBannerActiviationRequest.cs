using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.BannerSlides.Command.ToggleBannerActiviationCmd
{
    public record ToggleBannerActiviationRequest(Guid BannerID) : IRequest<Result<bool>>;
}
