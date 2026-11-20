using AutoMapper;
using E_Commerce_Inerno.Core.Communication;

using E_Commerce_Inerno.Core.RepositoryContracts.IWishListRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.WishListDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;

namespace E_Commerce_Inerno.Core.Features.WishLists.Commands.UpdateWishListCmd
{
    public class UpdateWishListHandler : IRequestHandler<UpdateWishListRequest, Result<WishListResponse>>
    {
        private readonly IWishListRepository _WishListRepo;
        private readonly IMapper _mapper;
        private readonly IInternoEventPublisher _Publisher;
        

        public UpdateWishListHandler(IWishListRepository WishListRepo, IInternoEventPublisher Publisher, IMapper mapper)
        {
            _WishListRepo = WishListRepo;
            _mapper = mapper;
            _Publisher = Publisher;
        }

        public async Task<Result<WishListResponse>> Handle(UpdateWishListRequest request, CancellationToken cancellationToken)
        {
            WishList? result = await _WishListRepo.GetWishList(request.WishlistID);
            if (result == null)
            {
                return Result<WishListResponse>.NotFound("WishList Wasnt Found");
            }
            string JsonOldValues = JsonSerializer.Serialize<WishList>(result);
            _mapper.Map(request, result);

            if (!await _WishListRepo.SaveChanges())
            {
                return Result<WishListResponse>.InternalError("Failed To SaveCHanges");
            }
            string JsonNewValues = JsonSerializer.Serialize<WishList>(result);
            AuditRequest AuditRequest = new(result.UserID, ActionTypeEnum.Update, nameof(WishList), JsonOldValues, JsonNewValues);
            await _Publisher.PublishAuditAsync(AuditRequest);
            return Result<WishListResponse>.Success(_mapper.Map<WishListResponse>(result));
        }
    }
}
