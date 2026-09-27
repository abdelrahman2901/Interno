using AutoMapper;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IWishListRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.WishListDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.WishLists.Query.GetAllWishListsQ
{
    public class GetAllUserWishListHandler : IRequestHandler<GetAllUserWishListQuery, Result<IEnumerable<WishListDetailsResponse>>>
    {
        private readonly IWishListRepository _WishListRepo;
        private readonly IMapper _mapper;

        public GetAllUserWishListHandler(IWishListRepository WishListRepo, IMapper mapper)
        {
            _WishListRepo = WishListRepo;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<WishListDetailsResponse>>> Handle(GetAllUserWishListQuery request, CancellationToken cancellationToken)
        {
            var result = await _WishListRepo.GetAllUserWishList(request.UserID);
            if (!result.Any())
            {
                return Result<IEnumerable<WishListDetailsResponse>>.NotFound("WishList Wasnt Found For this User");
            }
            return Result<IEnumerable<WishListDetailsResponse>>.Success(result.Select(s => _mapper.Map<WishListDetailsResponse>(s)));
        }
    }
}
