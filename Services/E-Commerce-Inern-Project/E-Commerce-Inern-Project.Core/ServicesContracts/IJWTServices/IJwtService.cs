using E_Commerce_Inern_Project.Core.DTO.AuthDTO;
using E_Commerce_Inern_Project.Core.Common;
using System.Security.Claims;
using E_Commerce_Inern_Project.Core.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.ServicesContracts.IJWTServices
{
    public interface IJwtService
    {
        Task<AuthTokenResponse> CreateJwtToken(ApplicationUser user);
        Result<ClaimsPrincipal?> GetPrincipalFromJwtToken(string? token);
    }
}
