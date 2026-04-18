using AutoMapper;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICartItemRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICartRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IProductRepo;
using E_Commerce_Inern_Project.Core.DTO.CartItemDTO;
using E_Commerce_Inern_Project.Core.Features.CartItem.Commands.CreateCartItemCmd;
using E_Commerce_Inern_Project.Core.Features.CartItem.Commands.UpdateCartItemCmd;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICartItemServices;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Services.CartItemServices
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _ProductRepo;
        private readonly IMapper _mapper;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly string _AuditRoutingKey = "Interno.Audit";
        public CartItemService(ICartItemRepository cartItemRepository, IRabbitMQPublisher Publisher, IProductRepository ProductRepo, ICartRepository cartRepository, IMapper mapper)
        {
            _cartItemRepository = cartItemRepository;
            _cartRepository = cartRepository;
            _ProductRepo = ProductRepo;
            _mapper = mapper;
            _Publisher = Publisher;
        }

        public async Task<Result<bool>> CreateCartItem(CreateCartItemRequest request)
        {
            Cart? UserCart = await _cartRepository.GetCartByUserID(request.UserID);
            if (UserCart == null)
            {
                return Result<bool>.NotFound("User Doesnt has Cart");
            }
            CartItems NewItem = _mapper.Map<CartItems>(request);
            NewItem.CartID = UserCart.CartID;

            CartItems? item = await _cartItemRepository.CreateCartItem(NewItem);
            if (item == null)
            {
                return Result<bool>.InternalError("Failed To Add Item");
            }

            if (!await _cartItemRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed to Save Changes");
            }

            string JsonNewValues = JsonSerializer.Serialize<CartItemResponse>(_mapper.Map<CartItemResponse>(item));
            AuditRequest AuditRequest = new(request.UserID, ActionTypeEnum.Create, nameof(CartItemResponse), null, JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            return Result<bool>.Success(item != null);
        }

        public async Task<Result<bool>> AddCartItemList(IEnumerable<CreateCartItemRequest> request)
        {
            Cart? UserCart = await _cartRepository.GetCartByUserID(request.FirstOrDefault().UserID);
            if (UserCart == null)
            {
                return Result<bool>.NotFound("User Doesnt has Cart");
            }

            List<CartItems> items = request.Select(r => _mapper.Map<CartItems>(r)).ToList();
            foreach (var item in items)
            {
                item.CartID = UserCart.CartID;
            }

            bool result = await _cartItemRepository.AddCartItemList(items);
            if (!result)
            {
                return Result<bool>.InternalError("Failed To Add Items");
            }


            if (!await _cartItemRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed to Save Changes");
            }
            string JsonNewValues = JsonSerializer.Serialize<List<CartItems>>(items);

            AuditRequest AuditRequest = new(request.FirstOrDefault().UserID, ActionTypeEnum.Create, nameof(CartItems), null, JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            return Result<bool>.Success(_mapper.Map<bool>(result));
        }

        public async Task<Result<bool>> DeleteCartItem(Guid ItemID)
        {
            CartItems? item = await _cartItemRepository.GetCartItem(ItemID);
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

        public async Task<Result<CartItemResponse>> UpdateCartItemQuantity(UpdateCartItemRequest request)
        {
            CartItems? item = await _cartItemRepository.GetCartItem(request.CartItemID);
            if (item == null)
            {
                return Result<CartItemResponse>.NotFound("Item Wasnt Found");
            }
            string JsonOldValues = JsonSerializer.Serialize<CartItemResponse>(_mapper.Map<CartItemResponse>(item));

            Product? product = await _ProductRepo.GetProductByID_NoTracking(item.ProductID);
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
