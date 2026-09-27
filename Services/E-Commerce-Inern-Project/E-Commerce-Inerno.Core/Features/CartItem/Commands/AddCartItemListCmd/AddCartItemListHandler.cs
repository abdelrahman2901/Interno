using AutoMapper;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.ICartItemRepo;
using E_Commerce_Inern_Project.Core.RepositoryContracts.ICartRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.CartItem.Commands.AddCartItemListCmd
{
    public class AddCartItemListHandler : IRequestHandler<AddCartItemListRequest, Result<bool>>
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly string _AuditRoutingKey = "Interno.Audit";

        public AddCartItemListHandler(ICartItemRepository cartItemRepository, IRabbitMQPublisher Publisher, ICartRepository cartRepository, IMapper mapper)
        {
            _cartItemRepository = cartItemRepository;
            _cartRepository = cartRepository;
            _mapper = mapper;
            _Publisher = Publisher;
        }

        public async Task<Result<bool>> Handle(AddCartItemListRequest request, CancellationToken cancellationToken)
        {
            Cart? UserCart = await _cartRepository.GetCartByUserID(request.UserID);
            if (UserCart == null)
            {
                return Result<bool>.NotFound("User Doesnt has Cart");
            }

            List<CartItems> items = request.ProductIDs.Select(r => _mapper.Map<CartItems>(r)).ToList();
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

            AuditRequest AuditRequest = new(request.UserID, ActionTypeEnum.Create, nameof(CartItems), null, JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            return Result<bool>.Success(_mapper.Map<bool>(result));
        }
    }
}
