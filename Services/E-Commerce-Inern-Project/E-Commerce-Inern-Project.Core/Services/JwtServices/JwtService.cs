using E_Commerce_Inern_Project.Core.DTO.AuthDTO;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Identity;
using E_Commerce_Inern_Project.Core.ServicesContracts.IJWTServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace E_Commerce_Inern_Project.Core.Services.JWTService
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtService> _logger;
        private readonly UserManager<ApplicationUser> _userManager; 
        public JwtService(IConfiguration configuration, ILogger<JwtService> logger, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<AuthTokenResponse> CreateJwtToken(ApplicationUser user)
        {
            DateTime TokenExpireTime= DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JWT:ExpireTime"]));
            string? role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            Claim[] TokenClaim =
            [
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Iat,DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            new (ClaimTypes.Name, user.UserName),
            new (ClaimTypes.Email, user.Email),
            new (ClaimTypes.Role, role)
            ];


           SymmetricSecurityKey securityKey= new (Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            SigningCredentials Credentials =new (securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken JwtToken = new JwtSecurityToken(
               issuer: _configuration["JWT:Issuer"],
               audience: _configuration["JWT:Audience"]
               ,claims:TokenClaim,
               signingCredentials:Credentials,
               expires:TokenExpireTime);

            JwtSecurityTokenHandler handler = new  ();
            string Token = handler.WriteToken(JwtToken);

            double RefreshTokenExpirationDays = double.Parse(_configuration["JWT:RefreshToken:ExpireTime"]);
            DateTime RefreshTokenExipireTime= DateTime.UtcNow.AddDays(RefreshTokenExpirationDays);

            return new AuthTokenResponse
            {
                PersonName = user.UserName,
                Email = user.Email,
                role = role,
                Token = Token,
                Expiration = TokenExpireTime,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpirationDateTime = RefreshTokenExipireTime
            };

        }

        public Result<ClaimsPrincipal?> GetPrincipalFromJwtToken(string? token)
        {
            TokenValidationParameters validators = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["JWT:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"])),
                ValidateLifetime = false,
                RoleClaimType = ClaimTypes.Role,

            };

            JwtSecurityTokenHandler handler = new ();
            ClaimsPrincipal principal = handler.ValidateToken(token, validators, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                Result<ClaimsPrincipal?>.BadRequest("invalid token");
            }
        
            return Result<ClaimsPrincipal?>.Success(principal);
        }

        private string GenerateRefreshToken()
        {
            byte[] bytes = new byte[64];
            var randomNumber = RandomNumberGenerator.Create();
            randomNumber.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
