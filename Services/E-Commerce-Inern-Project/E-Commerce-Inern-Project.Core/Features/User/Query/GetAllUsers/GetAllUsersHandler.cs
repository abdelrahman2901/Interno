using E_Commerce_Inern_Project.Core.DTO.UserDTO;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IUserServices;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.User.Query.GetAllUsers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<AuthUserDetailsDTO>>>
    {
        private readonly IUserService _UserService;
        public GetAllUsersHandler(IUserService UserService)
        {
            _UserService = UserService;
        }
        public async Task<Result<IEnumerable<AuthUserDetailsDTO>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await _UserService.GetAllUsers();
        }
    }
}
