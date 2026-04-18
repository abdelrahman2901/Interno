using E_Commerce_Inern_Project.Core.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace E_Commerce_Inern_Project.Core.Features.BannerSlide.Command.UpdateBannerCmd
{
    public class UpdateBannerRequest : IRequest<Result<bool>>
    {
        public Guid BannerSlideID { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string CTA { get; set; }
        public Guid BackgroundColorID { get; set; }
        public Guid AccentColorID { get; set; }
        public string Label { get; set; }
        public IFormFile? BannerImage { get; set; }
    }
}
