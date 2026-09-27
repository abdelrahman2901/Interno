using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.ICartRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.Carts.Commands.CreateCartCmd
{
    public class CreateCartHandler : IRequestHandler<CreateCartRequest, Result<bool>>
    {
        private readonly ICartRepository _cartRepo;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly string _AuditRoutingKey = "Interno.Audit";

        public CreateCartHandler(ICartRepository cartRepo, IRabbitMQPublisher Publisher)
        {
            _cartRepo = cartRepo;
            _Publisher = Publisher;
        }

        public async Task<Result<bool>> Handle(CreateCartRequest request, CancellationToken cancellationToken)
        {
            if (await _cartRepo.IsUserHasCart(request.UserID))
            {
                return Result<bool>.BadRequest("User Already Has Cart");
            }

            Cart cart = new() { CartID = Guid.NewGuid(), UserID = request.UserID, CreatedDate = DateTime.Now };
            bool result = await _cartRepo.CreateCart(cart);
            if (!result)
            {
                return Result<bool>.InternalError("Failed To Add Cart");
            }
            string JsonNewValues = JsonSerializer.Serialize<Cart>(cart);
            AuditRequest AuditRequest = new(request.UserID, ActionTypeEnum.Create, nameof(Cart), null, JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            return Result<bool>.Success(true);
        }
    }
}
