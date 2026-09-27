using E_Commerce_Interno.Domain.Enums;

namespace E_Commerce_Interno.Domain.DTO.UserDTO
{
    public class AuthUserDetailsDTO
    {
        public Guid userID { get; set; }
        public string? Email { get; set; }
        public string? PersonName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsBlocked { get; set; }
        public List<RolesEnum> Roles { get; set; } = [];
    }
}
