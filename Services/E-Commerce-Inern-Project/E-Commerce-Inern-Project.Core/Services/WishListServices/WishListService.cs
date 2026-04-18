using AutoMapper;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IWishListRepo;
using E_Commerce_Inern_Project.Core.DTO.WishListDTO;
using E_Commerce_Inern_Project.Core.Features.WishList.Commands.CreateWishListCmd;
using E_Commerce_Inern_Project.Core.Features.WishList.Commands.UpdateWishListCmd;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.ServicesContracts.IWishListServices;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Services.WishListServices
{
    public class WishListService : IWishListService
    {
        private readonly IWishListRepository _WishListRepo;
        private readonly IMapper _mapper;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly string _AuditRoutingKey = "Interno.Audit";
        public WishListService(IWishListRepository WishListRepo, IRabbitMQPublisher Publisher, IMapper mapper)
        {
            _WishListRepo = WishListRepo;
            _mapper = mapper;
            _Publisher = Publisher;
        }

        public async Task<Result<WishListResponse>> CreateWishList(CreateWishListRequest request)
        {
            WishList? wishList = await _WishListRepo.GetWishListByProdID(request.ProductID, request.UserID);
            if (wishList != null)
            {
                var Deleteresult = await DeleteWishList(wishList.WishlistID);
                if (!Deleteresult.IsSuccess)
                {
                    return Result<WishListResponse>.InternalError("Couldnt Delete Wishlist");
                }


                return Result<WishListResponse>.Success(new WishListResponse());
            }

            WishList? result = await _WishListRepo.CreateWishList(_mapper.Map<WishList>(request));
            if (result == null)
            {
                return Result<WishListResponse>.InternalError("Failed To Add WishList");
            }
            if (!await _WishListRepo.SaveChanges())
            {
                return Result<WishListResponse>.InternalError("Failed To SaveCHanges");
            }
            string JsonNewValues = JsonSerializer.Serialize<WishList>(result);
            AuditRequest AuditRequest = new(request.UserID, ActionTypeEnum.Create, nameof(WishList), null, JsonNewValues);
            await _Publisher.Publish<AuditRequest>(_AuditRoutingKey, AuditRequest);

            return Result<WishListResponse>.Success(_mapper.Map<WishListResponse>(result));
        }

        public async Task<Result<bool>> DeleteWishList(Guid WishListID)
        {
            WishList? result = await _WishListRepo.GetWishList(WishListID);
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
            await _Publisher.Publish<AuditRequest>(_AuditRoutingKey, AuditRequest);

            return Result<bool>.Success(true);
        }

        public async Task<Result<IEnumerable<WishListDetailsResponse>>> GetAllUserWishList(Guid UserID)
        {
            var result = await _WishListRepo.GetAllUserWishList(UserID);
            if (!result.Any())
            {
                return Result<IEnumerable<WishListDetailsResponse>>.NotFound("WishList Wasnt Found For this User");
            }
            return Result<IEnumerable<WishListDetailsResponse>>.Success(result.Select(s => _mapper.Map<WishListDetailsResponse>(s)));
        }

        public async Task<Result<bool>> ClearWishList(Guid UserID)
        {
            var WIshLists = await _WishListRepo.GetAllUserWishList(UserID);

            foreach (WishList item in WIshLists)
            {
                item.IsDeleted = true;
            }
            if (!await _WishListRepo.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaveChanges");
            }

            return Result<bool>.Success(true);
        }

        public async Task<Result<WishListResponse>> UpdateWishList(UpdateWishListRequest request)
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
            await _Publisher.Publish<AuditRequest>(_AuditRoutingKey, AuditRequest);
            return Result<WishListResponse>.Success(_mapper.Map<WishListResponse>(result));
        }
    }
}
