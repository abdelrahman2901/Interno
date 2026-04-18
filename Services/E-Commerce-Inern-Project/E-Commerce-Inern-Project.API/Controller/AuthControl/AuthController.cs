using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.AuthDTO;
using E_Commerce_Inern_Project.Core.DTO.TokenDTO;
using E_Commerce_Inern_Project.Core.DTO.UserDTO;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.BlockUser;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.ChangeUserPassword;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.CheckPassword;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.DeleteUser;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.LoginUser;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.RefreshToken;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.RegisterUser;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.SignOutUser;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.UpdateUser;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.UserResetPassword;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace E_Commerce_Inern_Project.API.Controller.AuthControl
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<Result<AuthTokenResponse>> RegisterUser(RegisterUserCommand Register)
        {
            return await _mediator.Send(Register);
        }

        [AllowAnonymous]
        [HttpPost("LoginUser")]
        [EnableRateLimiting("Auth")]
        public async Task<Result<AuthTokenResponse>> LoginUser(LoginUserCommand Login)
        {
            return await _mediator.Send(Login);
        }

        [AllowAnonymous]
        [HttpPost("SignOutUser/{UserID}")]
        public async Task<Result<bool>> SignOutUser(Guid UserID)
        {
            return await _mediator.Send(new SignOutUserCommand(UserID));
        }

        [Authorize]
        [HttpGet("CheckPassword")]
       public async Task<Result<bool>> CheckPassword([FromQuery]CheckPasswordCommand request)
        {
            return await _mediator.Send(request);
        }

        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<Result<AuthTokenResponse>> RefreshToken(TokenModelDTO tokenModel)
        {
            return await _mediator.Send(new RefreshTokenCommand(tokenModel));
        }

        [Authorize]
        [HttpPut("UpdateUser")]
        public async Task<Result<AuthUserDetailsDTO>> UpdateUser(UserUpdateRequest UpdateUserRequest)
        {
            return await _mediator.Send(new UpdateUserCommand(UpdateUserRequest));
        }

        [Authorize]
        [HttpPut("ChangeUserPassword")]
        public async Task<Result<bool>> ChangeUserPassword(ChangeUserPasswordCommand ChangeUserPasswordRequest)
        {
            return await _mediator.Send(ChangeUserPasswordRequest);
        }

        [Authorize]
        [HttpPut("UserResetPassword/{UserID}")]
        public async Task<Result<bool>> UserResetPassword(Guid UserID, string NewPassword)
        {
            return await _mediator.Send(new UserResetPasswordCommand(UserID, NewPassword));
        }

        [Authorize]
        [HttpPut("DeleteUser/{UserID}")]
        public async Task<Result<bool>> DeleteUser(Guid UserID)
        {
            return await _mediator.Send(new DeleteUserCommand(UserID));
        }

        [Authorize(Roles ="Admin")]
        [HttpPut("BlockUser/{UserID}")]
        public async Task<Result<bool>> BlockUser(Guid UserID)
        {
            return await _mediator.Send(new BlockUserCommand(UserID));
        }
    }
}
