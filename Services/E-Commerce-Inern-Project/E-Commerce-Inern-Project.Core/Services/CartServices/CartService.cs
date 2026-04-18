using AutoMapper;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICartRepo;
using E_Commerce_Inern_Project.Core.DTO.CartDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICartServices;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IMapper _mapper;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly string _AuditRoutingKey = "Interno.Audit";
        public CartService(ICartRepository cartRepo, IRabbitMQPublisher Publisher, IMapper mapper)
        {
            _cartRepo = cartRepo;
            _mapper = mapper;
            _Publisher = Publisher;
        }

        public async Task<Result<CartResponse>> CreateCart(Guid  UserID)
        {
            if(await _cartRepo.IsUserHasCart(UserID))
            {
                return Result<CartResponse>.BadRequest("User Already Has Cart");
            }

            Cart? result=await _cartRepo.CreateCart(new () { CartID = Guid.NewGuid(), UserID = UserID, CreatedDate = DateTime.Now });
            if (result == null)
            {
                return Result<CartResponse>.InternalError("Failed To Add Cart");
            }
            string JsonNewValues = JsonSerializer.Serialize<Cart>(result);
            AuditRequest AuditRequest = new(UserID, ActionTypeEnum.Create, nameof(Cart), null, JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            return Result<CartResponse>.Success(_mapper.Map<CartResponse>(result));
        }

        public async Task<Result<CartDetailsResponse>> GetUserCarItemsQuery(Guid UserID)
        {
            var result = await _cartRepo.GetUserCarItemsQuery(UserID);
            if (result==null)
            {
                return Result<CartDetailsResponse>.NotFound("no Items Was Found For this User");
            }
            return Result<CartDetailsResponse>.Success(_mapper.Map<CartDetailsResponse>(result));
        }
    }
}
