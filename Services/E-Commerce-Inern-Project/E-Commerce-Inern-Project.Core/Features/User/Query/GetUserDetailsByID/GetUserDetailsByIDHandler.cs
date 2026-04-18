using E_Commerce_Inern_Project.Core.DTO.UserDTO;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IUserServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.User.Query.GetUserDetailsByID
{
    public class GetUserDetailsByIDHandler : IRequestHandler<GetUserDetailsByIDQuery, Result<AuthUserDetailsDTO>>
    {
        private readonly IUserService _userService;
        public GetUserDetailsByIDHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<Result<AuthUserDetailsDTO>> Handle(GetUserDetailsByIDQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetUserDetailsByID(request.UserID);
        }
    }

}
