using E_Commerce_Interno.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Interno.Domain.Entity
{
    public class ApplicationUser
    {
        [Key]
        public Guid UserID { get; set; }
        public string PersonName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public GenderEnum? Gender { get; set; }
        public bool IsGmailVerified { get; set; }
        public bool IsPhoneNumberVerified { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsBlocked { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpirtation { get; set; }

        public Cart Cart{ get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<ProductRates> ProductRates { get; set; }
        public ICollection<WishList> WishLists { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Payments> Payments { get; set; }
        public ICollection<ApplicationUserRoles> Roles { get; set; }
    }
}
