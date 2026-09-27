using E_Commerce_Inern_Project.Core.Features.User.Query.GetAllUsers;
using E_Commerce_Inern_Project.Core.Features.User.Query.GetUserDetailsByEmail;
using E_Commerce_Inern_Project.Core.Features.User.Query.GetUserDetailsByID;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.UserDTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.UserControl.Queries
{
    [AllowAnonymous]
    [Route("api/User")]
    [ApiController]
    public class UserQueriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserQueriesController(IMediator mediator)
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
