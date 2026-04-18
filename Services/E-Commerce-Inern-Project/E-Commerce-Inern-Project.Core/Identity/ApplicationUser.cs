using E_Commerce_Inern_Project.Core.Domain.Entity;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce_Inern_Project.Core.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string PersonName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpirtation { get; set; }
        public bool IsDeleted { get; set; }= false;
        public bool IsBlocked { get; set; }=false;
        

        public ICollection<Address> Addresses { get; set; }
        
        public ICollection<ProductRates> ProductRates { get; set; }
        public ICollection<WishList> WishLists { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Payments> Payments { get; set; }
    }
}
