 
 

namespace E_Commerce_Inern_Project.Core.DTO.AuthDTO
{
    public class AuthTokenResponse
    {
        public string? PersonName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? role { get; set; } = string.Empty;
        public string? Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string? RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpirationDateTime { get; set; }

    }
}
