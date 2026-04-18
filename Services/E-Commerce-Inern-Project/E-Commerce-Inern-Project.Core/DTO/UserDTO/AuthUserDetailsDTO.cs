namespace E_Commerce_Inern_Project.Core.DTO.UserDTO
{
    public class AuthUserDetailsDTO
    {
        public Guid userID { get; set; }
        public string? Email { get; set; }
        public string? PersonName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }
        public bool IsBlocked { get; set; }
    }

}
