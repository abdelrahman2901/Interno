namespace E_Commerce_Interno.Domain.DTO.AuthDTO
{
    public class AuthTokenResponse
    {
        public string? PersonName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string? RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpirationDateTime { get; set; }
        public IEnumerable<string>? roles { get; set; } = [];
    }
}
