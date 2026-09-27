using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.BannerSlides.Command.ToggleBannerActiviationCmd
{
    public record ToggleBannerActiviationRequest(Guid BannerID) : IRequest<Result<bool>>;
}
