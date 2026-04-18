using E_Commerce_Inern_Project.Core.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.BannerSlide.Command.ToggleBannerActiviationCmd
{
    public record ToggleBannerActiviationRequest(Guid BannerID) : IRequest<Result<bool>>;
}
