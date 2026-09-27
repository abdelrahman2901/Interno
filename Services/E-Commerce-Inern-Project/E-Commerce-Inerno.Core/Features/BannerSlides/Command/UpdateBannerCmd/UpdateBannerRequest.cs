using E_Commerce_Interno.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace E_Commerce_Inern_Project.Core.Features.BannerSlides.Command.UpdateBannerCmd
{
    public class UpdateBannerRequest : IRequest<Result<bool>>
    {
        public Guid BannerSlideID { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string CTA { get; set; }

        [SwaggerSchema(Nullable = true)]
        public Guid? BackgroundColorID { get; set; }
        [SwaggerSchema(Nullable = true)]
        public string? BackgroundColorHex { get; set; }
        [SwaggerSchema(Nullable = true)]
        public Guid? AccentColorID { get; set; }
        [SwaggerSchema(Nullable = true)]
        public string? AccentColorHex { get; set; }

        public string Label { get; set; }
        public IFormFile? BannerImage { get; set; }
    }
}
