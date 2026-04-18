using AutoMapper;
using E_Commerce_Inern_Project.Core.DTO.UserDTO;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IUserRepo;
using E_Commerce_Inern_Project.Core.ServicesContracts.IUserServices;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IAuthRepo;

namespace E_Commerce_Inern_Project.Core.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _UserRepo;
        private readonly IMapper _mapper;
        public UserService(IUserRepository UserRepo,IMapper mapper)
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
            return Result<IEnumerable<AuthUserDetailsDTO>>.Success(users);
        }
         
        //public async Task<Result<ApplicationUser>> GetApplicationUserByEmail(string email)
        //{
        //    var user = await _UserRepo.GetApplicationUserByEmail(email);
        //    if (user ==null)
        //    {
        //        return Result<ApplicationUser>.NotFound("User Doenst Exists");
        //    }
        //    return Result<ApplicationUser>.Success(user);

        //}

        //public async Task<Result<ApplicationUser>> GetApplicationUserByID(Guid userid)
        //{
        //    var user = await _UserRepo.GetApplicationUserByID(userid);
        //    if (user == null)
        //    {
        //        return Result<ApplicationUser>.NotFound("User Doenst Exists");
        //    }
        //    return Result<ApplicationUser>.Success(user);
        //}
        public async Task<Result<AuthUserDetailsDTO>> GetUserDetailsByEmail(string email)
        {
            var user = await _UserRepo.GetApplicationUserByEmail(email);
            if (user == null)
            {
                return Result<AuthUserDetailsDTO>.NotFound("User Doenst Exists");
            }
            AuthUserDetailsDTO userDetails = _mapper.Map<AuthUserDetailsDTO>(user);
            userDetails.Role = await _UserRepo.GetUserRole(user);
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
            userDetails.Role = await _UserRepo.GetUserRole(user);
            return Result<AuthUserDetailsDTO>.Success(userDetails);
        }

        //public async Task<Result<bool>> IsAccountDeleted(string Email)
        //{
        //    ApplicationUser? user = await _UserRepo.GetApplicationUserByEmail(Email);
        //    if (user == null)
        //    {
        //        return Result<bool>.NotFound("User Doenst Exists");
        //    }

        //    return Result<bool>.Success(user.IsDeleted);
        //}

        //public async Task<Result<bool>> IsEmailAlreadyRegister(string Email)
        //{
        //    bool result = await _UserRepo.IsEmailAlreadyRegister(Email);
        //    if (!result)
        //    {
        //        return Result<bool>.NotFound("Email is not Registered");
        //    }
        //    return Result<bool>.Success(result);
        //}
    }
}
