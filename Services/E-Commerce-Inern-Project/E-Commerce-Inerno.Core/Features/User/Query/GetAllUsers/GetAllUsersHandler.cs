using E_Commerce_Inerno.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.UserDTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Inerno.Core.Features.User.Query.GetAllUsers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, Result<PagedResult<AuthUserDetailsDTO>>>
    {
        private readonly IUserRepository _UserRepo;

        public GetAllUsersHandler(IUserRepository UserRepo)
        {
            _UserRepo = UserRepo;
        }

        public async Task<Result<PagedResult<AuthUserDetailsDTO>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            var pageSize = request.PageSize < 1 ? 20 : request.PageSize;

            var query = _UserRepo.GetAllUsersQuery();
            var totalCount = await query.CountAsync(cancellationToken);
            if (totalCount == 0)
            {
                return Result<PagedResult<AuthUserDetailsDTO>>.NotFound("No users found.");
            }

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return Result<PagedResult<AuthUserDetailsDTO>>.Success(
                PagedResult<AuthUserDetailsDTO>.Create(items, totalCount, pageNumber, pageSize));
        }
    }
}
