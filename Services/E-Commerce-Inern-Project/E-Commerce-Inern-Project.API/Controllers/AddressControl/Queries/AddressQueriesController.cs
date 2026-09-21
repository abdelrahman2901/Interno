using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Features.Address.Query.GetUserAddressByIDQuery;
using E_Commerce_Inern_Project.Core.Features.Address.Query.GetUserAddressQuery;
using E_Commerce_Interno.Domain.DTO.AddressDTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce_Inern_Project.API.Controllers.AddressControl.Queries
{
    [Authorize]
    [Route("api/Address")]
    [ApiController]
    public class AddressQueriesController : ControllerBase
    {
        private readonly IMediator _MediatR;

        public AddressQueriesController(IMediator mediatR)
        {
            _MediatR = mediatR;
        }

        [HttpGet("[action]")]
        public async Task<Result<IEnumerable<AddressDetails>>> GetUserAddress()
        {
            string? UserStrID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(UserStrID))
            {
                return Result<IEnumerable<AddressDetails>>.UnAuthorized("User not Authorized");
            }
            var UserID = Guid.Parse(UserStrID);
            return await _MediatR.Send(new GetUserAddressQ(UserID));
        }

        [HttpGet("[action]/{AddressID}")]
        public async Task<Result<AddressResponse>> GetUserAddressByID(Guid AddressID)
        {
            return await _MediatR.Send(new GetUserAddressByIDQ(AddressID));
        }
    }
}
