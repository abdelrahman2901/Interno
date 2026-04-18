using E_Commerce_Inern_Project.Core.DTO.ColorDTO;

namespace E_Commerce_Inern_Project.Core.DTO.BannerSlideDTO
{
    public class BannerSlideResponse
    {
        public Guid BannerSlideID { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string CTA { get; set; }
        public ColorResponse BackgroundColor { get; set; }
        public ColorResponse AccentColor { get; set; }
        public string Label { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
}