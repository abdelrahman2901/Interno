using AutoMapper;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.ICartItemRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CartItemDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.CartItem.Commands.DeleteCartItemCmd
{
    public class DeleteCartItemHandler : IRequestHandler<DeleteCartItemRequest, Result<bool>>
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IMapper _mapper;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly string _AuditRoutingKey = "Interno.Audit";

        public DeleteCartItemHandler(ICartItemRepository cartItemRepository, IRabbitMQPublisher Publisher, IMapper mapper)
        {
            _cartItemRepository = cartItemRepository;
            _mapper = mapper;
            _Publisher = Publisher;
        }

        public async Task<Result<bool>> Handle(DeleteCartItemRequest request, CancellationToken cancellationToken)
        {
            CartItems? item = await _cartItemRepository.GetCartItem(request.ItemID);
            if (item == null)
            {
                return Result<bool>.NotFound("Item Wasnt Found");
            }
            string JsonOldValues = JsonSerializer.Serialize<CartItemResponse>(_mapper.Map<CartItemResponse>(item));
            item.IsDeleted = true;
            if (!await _cartItemRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed to Save Changes");
            }
            string JsonNewValues = JsonSerializer.Serialize<CartItemResponse>(_mapper.Map<CartItemResponse>(item));

            AuditRequest AuditRequest = new(item.Cart.UserID, ActionTypeEnum.Delete, nameof(CartItemResponse), JsonOldValues, JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            return Result<bool>.Success(true);
        }
    }
}
