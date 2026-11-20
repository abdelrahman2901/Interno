using MediatR;
using E_Commerce_Interno.Domain.DTO.TokenDTO;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AuthDTO;

namespace E_Commerce_Inerno.Core.Features.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand(TokenModelDTO TokenModel) : IRequest<Result<AuthTokenResponse>>;
}
