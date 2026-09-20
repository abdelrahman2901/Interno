using AutoMapper;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IUserRepo;
using E_Commerce_Inern_Project.Core.DTO.UserDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IUserServices;

namespace E_Commerce_Inern_Project.Core.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _UserRepo;
        private readonly IMapper _mapper;
        public UserService(IUserRepository UserRepo, IMapper mapper)
        {
            _UserRepo = UserRepo;

            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<AuthUserDetailsDTO>>> GetAllUsers()
        {
            var users = await _UserRepo.GetAllUsers();
            if (!users.Any())
            {
                return Result<IEnumerable<AuthUserDetailsDTO>>.NotFound("No users found.");
            }
            return Result<IEnumerable<AuthUserDetailsDTO>>.Success(users.Select(u => _mapper.Map<AuthUserDetailsDTO>(u)));
        }

        public async Task<Result<AuthUserDetailsDTO>> GetUserDetailsByEmail(string email)
        {
            var user = await _UserRepo.GetApplicationUserByEmail(email);
            if (user == null)
            {
                return Result<AuthUserDetailsDTO>.NotFound("User Doenst Exists");
            }
            AuthUserDetailsDTO userDetails = _mapper.Map<AuthUserDetailsDTO>(user);
            return Result<AuthUserDetailsDTO>.Success(userDetails);
        }

        public async Task<Result<AuthUserDetailsDTO>> GetUserDetailsByID(Guid userid)
        {
            var user = await _UserRepo.GetApplicationUserByID(userid);
            if (user == null)
            {
                return Result<AuthUserDetailsDTO>.NotFound("User Doenst Exists");
            }
            AuthUserDetailsDTO userDetails = _mapper.Map<AuthUserDetailsDTO>(user);
            return Result<AuthUserDetailsDTO>.Success(userDetails);
        }
 
    }
}
