
using System.ComponentModel.DataAnnotations;


namespace E_Commerce_Inern_Project.Core.Domain.Entity
{
    public class City
    {
        [Key]
        [MaxLength(50)]
        public Guid CityID { get; set; }
        [MaxLength(100)]
        public string CityName { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<Area> Areas { get; set; }
        public ICollection<Address> Addresses { get; set; }

    }
}
