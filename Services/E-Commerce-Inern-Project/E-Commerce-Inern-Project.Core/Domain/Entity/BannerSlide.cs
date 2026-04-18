using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Inern_Project.Core.Domain.Entity
{
    public class BannerSlide
    {
        public Guid BannerSlideID { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(200)]
        public string Subtitle { get; set; }

        [MaxLength(100)]
        public string CTA { get; set; }

        [MaxLength(200)]
        public Guid BackgroundColorID { get; set; }
        [ForeignKey(nameof(BackgroundColorID))]
        public Colors BackgroundColor { get; set; }
        [MaxLength(20)]
        public Guid AccentColorID { get; set; }
        [ForeignKey(nameof(AccentColorID))]
        public Colors AccentColor { get; set; }

        [MaxLength(50)]
        public string Label { get; set; }

        public string ImageUrl { get; set; }
        public string ImageHash { get; set; }
        public bool IsDeleted { get; set; }=false;
        public bool IsActive { get; set; } = false;
    }
}
