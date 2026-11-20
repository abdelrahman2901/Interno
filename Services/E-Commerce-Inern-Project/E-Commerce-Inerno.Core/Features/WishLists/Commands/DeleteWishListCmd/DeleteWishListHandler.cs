using E_Commerce_Inerno.Core.Communication;

using E_Commerce_Inerno.Core.RepositoryContracts.IWishListRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;

namespace E_Commerce_Inerno.Core.Features.WishLists.Commands.DeleteWishListCmd
{
    public class DeleteWishListHandler : IRequestHandler<DeleteWishListRequest, Result<bool>>
    {
        private readonly IWishListRepository _WishListRepo;
        private readonly IInternoEventPublisher _Publisher;
        

        public DeleteWishListHandler(IWishListRepository WishListRepo, IInternoEventPublisher Publisher)
        {
            _WishListRepo = WishListRepo;
            _Publisher = Publisher;
        }

        public async Task<Result<bool>> Handle(DeleteWishListRequest request, CancellationToken cancellationToken)
        {
            WishList? result = await _WishListRepo.GetWishList(request.WishList);
            if (result == null)
            {
                return Result<bool>.NotFound("WishList Wasnt Found");
            }
            string JsonOldValues = JsonSerializer.Serialize<WishList>(result);

            result.IsDeleted = true;

            if (!await _WishListRepo.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaveCHanges");
            }

            string JsonNewValues = JsonSerializer.Serialize<WishList>(result);
            AuditRequest AuditRequest = new(result.UserID, ActionTypeEnum.Delete, nameof(WishList), JsonOldValues, JsonNewValues);
            await _Publisher.PublishAuditAsync(AuditRequest);

            return Result<bool>.Success(true);
        }
    }
}
