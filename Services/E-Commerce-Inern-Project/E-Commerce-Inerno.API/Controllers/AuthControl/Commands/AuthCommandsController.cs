
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.BlockUser;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.ChangeUserPassword;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.DeleteUser;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.LoginUser;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.RefreshToken;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.RegisterUser;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.SignOutUser;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.UpdateUser;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.UserResetPassword;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AuthDTO;
using E_Commerce_Interno.Domain.DTO.TokenDTO;
using E_Commerce_Interno.Domain.DTO.UserDTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace E_Commerce_Inern_Project.API.Controllers.AuthControl.Commands
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthCommandsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthCommandsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<Result<AuthTokenResponse>> RegisterUser(RegisterUserCommand Register)
        {
            return await _mediator.Send(Register);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        [EnableRateLimiting("Auth")]
        public async Task<Result<AuthTokenResponse>> LoginUser(LoginUserCommand Login)
        {
            return await _mediator.Send(Login);
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<Result<bool>> SignOutUser()
        {
            var UserStrID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(UserStrID))
            {
                return Result<bool>.UnAuthorized("User not Authorized");
            }

            var UserID = Guid.Parse(UserStrID);
            return await _mediator.Send(new SignOutUserCommand(UserID));
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<Result<AuthTokenResponse>> RefreshToken(TokenModelDTO tokenModel)
        {
            return await _mediator.Send(new RefreshTokenCommand(tokenModel));
        }

        [Authorize]
        [HttpPut("[action]")]
        public async Task<Result<AuthUserDetailsDTO>> UpdateUser(UpdateUserCommand UpdateUserRequest)
        {
            var UserStrID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(UserStrID))
            {
                return Result<AuthUserDetailsDTO>.UnAuthorized("User not Authorized");
            }

            UpdateUserRequest.UserID = Guid.Parse(UserStrID);
            return await _mediator.Send(UpdateUserRequest);
        }

        [Authorize]
        [HttpPut("[action]")]
        public async Task<Result<bool>> ChangeUserPassword(ChangeUserPasswordCommand ChangeUserPasswordRequest)
        {
            var UserStrID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(UserStrID))
            {
                return Result<bool>.UnAuthorized("User not Authorized");
            }

            ChangeUserPasswordRequest.UserID = Guid.Parse(UserStrID);
            return await _mediator.Send(ChangeUserPasswordRequest);
        }

        [Authorize]
        [HttpPut("[action]")]
        public async Task<Result<bool>> UserResetPassword(string NewPassword)
        {
            var UserStrID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(UserStrID))
            {
                return Result<bool>.UnAuthorized("User not Authorized");
            }

            var UserID = Guid.Parse(UserStrID);

            return await _mediator.Send(new UserResetPasswordCommand(UserID, NewPassword));
        }

        [Authorize]
        [HttpPut("DeleteUser")]
        public async Task<Result<bool>> DeleteUser()
        {
            var UserStrID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(UserStrID))
            {
                return Result<bool>.UnAuthorized("User not Authorized");
            }

            var UserID = Guid.Parse(UserStrID);
            return await _mediator.Send(new DeleteUserCommand(UserID));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("BlockUser/{UserID}")]
        public async Task<Result<bool>> BlockUser(Guid UserID)
        {
            return await _mediator.Send(new BlockUserCommand(UserID));
        }
    }
}
