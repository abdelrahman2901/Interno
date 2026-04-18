using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.AddressDTO;
using E_Commerce_Inern_Project.Core.Features.Address.Commands.CreateAddressCmd;
using E_Commerce_Inern_Project.Core.Features.Address.Commands.DeleteAddressCmd;
using E_Commerce_Inern_Project.Core.Features.Address.Commands.UpdateAddressCmd;
using E_Commerce_Inern_Project.Core.Features.Address.Query.GetUserAddressByIDQuery;
using E_Commerce_Inern_Project.Core.Features.Address.Query.GetUserAddressQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controller.AddressControl
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IMediator _MediatR;
        public AddressController(IMediator mediatR)
        {
            _MediatR = mediatR;
        }


        [HttpGet("GetUserAddress/{UserID}")]
        public async Task<Result<IEnumerable<AddressDetails>>> GetUserAddress(Guid UserID)
        {
            return await _MediatR.Send(new GetUserAddressQ(UserID));
        }
        [HttpGet("GetUserAddressByID/{AddressID}")]
        public async Task<Result<AddressResponse>> GetUserAddressByID(Guid AddressID)
        {
            return await _MediatR.Send(new GetUserAddressByIDQ(AddressID));
        }

        [HttpPost("CreateAddress")]
        public async Task<Result<AddressResponse>> CreateAddress(CreateAddressRequest request)
        {
            return await _MediatR.Send(request);
        }
        [HttpPut("DeleteAddress/{AddressID}")]
        public async Task<Result<bool>> DeleteAddress(Guid AddressID)
        {
            return await _MediatR.Send(new DeleteAddressCmd(AddressID));
        }
        [HttpPut("UpdateAddress")]
        public async Task<Result<AddressResponse>> UpdateAddress(UpdateAddressRequest request)
        {
            return await _MediatR.Send(request);
        }
    }
}
