using E_Commerce_Inern_Project.Core.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Inern_Project.Core.Domain.Entity
{
    public class Address
    {
        [Key]
        [MaxLength(100)]
        public Guid AddressID { get; set; }
        [MaxLength(100)]
        public Guid UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public ApplicationUser User { get; set; }
        [MaxLength(250)]
        public string AddressLabel { get; set; }
        [MaxLength(500)]
        public string MainAddress { get; set; }
        [MaxLength(500)]
        public string? BackUpAddress { get; set; }
        [MaxLength(100)]
        public Guid CityID { get; set; }
        [ForeignKey(nameof(CityID))]
        public City City { get; set; }

        [MaxLength(100)]
        public Guid AreaID { get; set; }
        [ForeignKey(nameof(AreaID))]
        public Area Area { get; set; }
        [MaxLength(12)]
        public string BackUpPhoneNumber { get; set; }

        public bool IsDeleted { get; set; } = false;
        public bool IsDefault { get; set; } = false;

       public ICollection<Order> orders { get; set; }
    }
}
