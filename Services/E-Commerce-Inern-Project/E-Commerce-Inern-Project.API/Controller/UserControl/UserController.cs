using E_Commerce_Inern_Project.Core.DTO.UserDTO;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Features.User.Query.GetAllUsers;
using E_Commerce_Inern_Project.Core.Features.User.Query.GetUserDetailsByEmail;
using E_Commerce_Inern_Project.Core.Features.User.Query.GetUserDetailsByID;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controller.UserControl
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<Result<IEnumerable<AuthUserDetailsDTO>>> GetAllUsers()
        {
            return await _mediator.Send(new GetAllUsersQuery());
        }
        [HttpGet("GetUserDetailsByEmail")]
        public async Task<Result<AuthUserDetailsDTO>> GetUserDetailsByEmail(string email)
        {
            return await _mediator.Send(new GetUserDetailsByEmailQuery(email));
        }
        [HttpGet("GetUserDetailsByID/{UserID}")]
        public async Task<Result<AuthUserDetailsDTO>> GetUserDetailsByID(Guid UserID)
        {
            return await _mediator.Send(new GetUserDetailsByIDQuery(UserID));
        }

    }
}
