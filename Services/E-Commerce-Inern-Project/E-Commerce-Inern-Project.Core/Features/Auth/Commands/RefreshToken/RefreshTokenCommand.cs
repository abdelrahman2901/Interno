using E_Commerce_Inern_Project.Core.DTO.AuthDTO;
using E_Commerce_Inern_Project.Core.Common;
using MediatR;
using E_Commerce_Interno.Domain.DTO.TokenDTO;

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand(TokenModelDTO TokenModel) : IRequest<Result<AuthTokenResponse>>;
}
