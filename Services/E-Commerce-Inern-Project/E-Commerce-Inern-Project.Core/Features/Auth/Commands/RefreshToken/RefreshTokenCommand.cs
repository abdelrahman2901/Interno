using E_Commerce_Inern_Project.Core.DTO.AuthDTO;
using E_Commerce_Inern_Project.Core.DTO.TokenDTO;
using E_Commerce_Inern_Project.Core.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand(TokenModelDTO TokenModel) : IRequest<Result<AuthTokenResponse>>;
}
