using AutoMapper;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.ICartItemRepo;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IProductRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CartItemDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.CartItem.Commands.UpdateCartItemCmd
{
    public class UpdateCartItemHandler : IRequestHandler<UpdateCartItemRequest, Result<CartItemResponse>>
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _ProductRepo;
        private readonly IMapper _mapper;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly string _AuditRoutingKey = "Interno.Audit";

        public UpdateCartItemHandler(ICartItemRepository cartItemRepository, IRabbitMQPublisher Publisher, IProductRepository ProductRepo, IMapper mapper)
        {
            _cartItemRepository = cartItemRepository;
            _ProductRepo = ProductRepo;
            _mapper = mapper;
            _Publisher = Publisher;
        }

        public async Task<Result<CartItemResponse>> Handle(UpdateCartItemRequest request, CancellationToken cancellationToken)
        {
            CartItems? item = await _cartItemRepository.GetCartItem(request.CartItemID);
            if (item == null)
            {
                return Result<CartItemResponse>.NotFound("Item Wasnt Found");
            }
            string JsonOldValues = JsonSerializer.Serialize<CartItemResponse>(_mapper.Map<CartItemResponse>(item));

            Product? product = await _ProductRepo.GetProductByID(item.ProductID, false);
            if (product == null)
            {
                return Result<CartItemResponse>.NotFound("Product Doesnt Exists");
            }

            if (item.Quantity == 0 && !request.IsIncrease)
            {
                return Result<CartItemResponse>.BadRequest($"ACtion Denied");
            }

            if (product.Stock == item.Quantity && request.IsIncrease)
            {
                return Result<CartItemResponse>.BadRequest($"Item is Out Of Stock You Cant Order More than {product.Stock}");
            }

            item.Quantity = request.IsIncrease ? ++item.Quantity : --item.Quantity;

            if (!await _cartItemRepository.SaveChanges())
            {
                return Result<CartItemResponse>.InternalError("Failed to Save Changes");
            }
            string JsonNewValues = JsonSerializer.Serialize<CartItemResponse>(_mapper.Map<CartItemResponse>(item));

            AuditRequest AuditRequest = new(item.Cart.UserID, ActionTypeEnum.Update, nameof(CartItemResponse), JsonOldValues, JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            return Result<CartItemResponse>.Success(_mapper.Map<CartItemResponse>(item));
        }
    }
}
