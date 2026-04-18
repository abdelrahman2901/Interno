using E_Commerce_Inern_Project.Core.DTO.AuthDTO;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Identity;
using System.Security.Claims;

namespace E_Commerce_Inern_Project.Core.ServicesContracts.IJWTServices
{
    public interface IJwtService
    {
        Task<AuthTokenResponse> CreateJwtToken(ApplicationUser user);
        Result<ClaimsPrincipal?> GetPrincipalFromJwtToken(string? token);
    }
}
