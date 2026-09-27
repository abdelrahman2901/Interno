using AutoMapper;
using E_Commerce_Inern_Project.Core.Features.WishLists.Commands.DeleteWishListCmd;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IWishListRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.WishLists.Commands.CreateWishListCmd
{
    public class CreateWishListHandler : IRequestHandler<CreateWishListRequest, Result<bool>>
    {
        private readonly IWishListRepository _WishListRepo;
        private readonly IMapper _mapper;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly IMediator _mediator;
        private readonly string _AuditRoutingKey = "Interno.Audit";

        public CreateWishListHandler(IWishListRepository WishListRepo, IRabbitMQPublisher Publisher, IMediator mediator, IMapper mapper)
        {
            _WishListRepo = WishListRepo;
            _mapper = mapper;
            _Publisher = Publisher;
            _mediator = mediator;
        }

        public async Task<Result<bool>> Handle(CreateWishListRequest request, CancellationToken cancellationToken)
        {
            WishList? existing = await _WishListRepo.GetWishListByProdID(request.ProductID, request.UserID);
            if (existing != null)
            {
                var Deleteresult = await _mediator.Send(new DeleteWishListRequest(existing.WishlistID), cancellationToken);
                if (!Deleteresult.IsSuccess)
                {
                    return Result<bool>.InternalError("Couldnt Delete Wishlist");
                }

                return Result<bool>.Success(true);
            }

            WishList wishlist = _mapper.Map<WishList>(request);
            bool result = await _WishListRepo.CreateWishList(wishlist);
            if (!result)
            {
                return Result<bool>.InternalError("Failed To Add WishList");
            }
            if (!await _WishListRepo.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaveCHanges");
            }
            string JsonNewValues = JsonSerializer.Serialize<WishList>(wishlist);
            AuditRequest AuditRequest = new(request.UserID, ActionTypeEnum.Create, nameof(WishList), null, JsonNewValues);
            await _Publisher.Publish<AuditRequest>(_AuditRoutingKey, AuditRequest);

            return Result<bool>.Success(true);
        }
    }
}
