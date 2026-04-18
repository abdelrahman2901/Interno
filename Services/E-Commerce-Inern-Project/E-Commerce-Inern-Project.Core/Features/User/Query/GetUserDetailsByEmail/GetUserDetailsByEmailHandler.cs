using E_Commerce_Inern_Project.Core.DTO.UserDTO;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IUserServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.User.Query.GetUserDetailsByEmail
{
    public class GetUserDetailsByEmailHandler : IRequestHandler<GetUserDetailsByEmailQuery, Result<AuthUserDetailsDTO>>
    {
        private readonly IUserService _UserService; 
        public GetUserDetailsByEmailHandler(IUserService UserService)
        {
            _UserService = UserService;
        }
        public async Task<Result<AuthUserDetailsDTO>> Handle(GetUserDetailsByEmailQuery request, CancellationToken cancellationToken)
        {
            return await _UserService.GetUserDetailsByEmail(request.Email);
        }
    }

}
