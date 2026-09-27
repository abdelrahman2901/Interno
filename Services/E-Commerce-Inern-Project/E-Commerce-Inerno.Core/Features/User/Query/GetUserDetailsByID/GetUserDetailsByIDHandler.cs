using AutoMapper;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.UserDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.User.Query.GetUserDetailsByID
{
    public class GetUserDetailsByIDHandler : IRequestHandler<GetUserDetailsByIDQuery, Result<AuthUserDetailsDTO>>
    {
        private readonly IUserRepository _UserRepo;
        private readonly IMapper _mapper;

        public GetUserDetailsByIDHandler(IUserRepository UserRepo, IMapper mapper)
        {
            _UserRepo = UserRepo;
            _mapper = mapper;
        }

        public async Task<Result<AuthUserDetailsDTO>> Handle(GetUserDetailsByIDQuery request, CancellationToken cancellationToken)
        {
            var user = await _UserRepo.GetApplicationUserByID(request.UserID, false);
            if (user == null)
            {
                return Result<AuthUserDetailsDTO>.NotFound("User Doenst Exists");
            }
            AuthUserDetailsDTO userDetails = _mapper.Map<AuthUserDetailsDTO>(user);
            return Result<AuthUserDetailsDTO>.Success(userDetails);
        }
    }
}
