using E_Commerce_Inerno.Core.Features.Addresses.Commands.DeleteAddressCmd;
using E_Commerce_Inerno.Core.Features.Addresses.Commands.CreateAddressCmd;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AddressDTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using E_Commerce_Inerno.Core.Features.Addresses.Commands.UpdateAddressCmd;

namespace E_Commerce_Inerno.API.Controllers.AddressControl.Commands
{
    [Authorize]
    [Route("api/Address")]
    [ApiController]
    public class AddressCommandsController : ControllerBase
    {
        private readonly IMediator _MediatR;

        public AddressCommandsController(IMediator mediatR)
        {
            _MediatR = mediatR;
        }

        [HttpPost("[action]")]
        public async Task<Result<AddressResponse>> CreateAddress(CreateAddressRequest request)
        {
            string? UserStrID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(UserStrID))
            {
                return Result<AddressResponse>.UnAuthorized("User not Authorized");
            }
            request.UserID = Guid.Parse(UserStrID);
            return await _MediatR.Send(request);
        }

        [HttpPut("[action]/{AddressID}")]
        public async Task<Result<bool>> DeleteAddress(Guid AddressID)
        {
            return await _MediatR.Send(new DeleteAddressCmd(AddressID));
        }

        [HttpPut("[action]")]
        public async Task<Result<AddressResponse>> UpdateAddress(UpdateAddressRequest request)
        {
            return await _MediatR.Send(request);
        }
    }
}
