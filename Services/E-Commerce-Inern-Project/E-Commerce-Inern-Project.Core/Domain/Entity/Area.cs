using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace E_Commerce_Inern_Project.Core.Domain.Entity
{
    public class Area
    {
        [Key]
        [MaxLength(50)]
        public Guid AreaID { get; set; }
        [Required(ErrorMessage = "City ID is required")]
        [MaxLength(50)]
        public Guid CityID { get; set; }
        [ForeignKey(nameof(CityID))]
        public City City { get; set; }
        [Required(ErrorMessage = "Area Name is required")]
        [MaxLength(100)]
        public string AreaName { get; set; }
        public bool IsDeleted { get; set; }=false;

        public ICollection<Address> Addresses { get; set; }
        public ShippingCosts ShippingCosts { get; set; }
    }
}
