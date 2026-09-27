using AutoMapper;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.UserDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.User.Query.GetAllUsers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<AuthUserDetailsDTO>>>
    {
        private readonly IUserRepository _UserRepo;
        private readonly IMapper _mapper;

        public GetAllUsersHandler(IUserRepository UserRepo, IMapper mapper)
        {
            _UserRepo = UserRepo;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<AuthUserDetailsDTO>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _UserRepo.GetAllUsers();
            if (!users.Any())
            {
                return Result<IEnumerable<AuthUserDetailsDTO>>.NotFound("No users found.");
            }
            return Result<IEnumerable<AuthUserDetailsDTO>>.Success(users.Select(u => _mapper.Map<AuthUserDetailsDTO>(u)));
        }
    }
}
