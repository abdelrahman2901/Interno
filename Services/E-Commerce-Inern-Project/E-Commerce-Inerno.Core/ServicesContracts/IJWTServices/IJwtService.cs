using E_Commerce_Interno.Domain.DTO.AuthDTO; 
using System.Security.Claims;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inerno.Core.ServicesContracts.IJWTServices
{
    public interface IJwtService
    {
        Task<AuthTokenResponse> CreateJwtToken(ApplicationUser user);
        Result<ClaimsPrincipal?> GetPrincipalFromJwtToken(string? token);
    }
}
