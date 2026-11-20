using E_Commerce_Inerno.Core.Communication;

using E_Commerce_Inerno.Core.RepositoryContracts.ICartRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;

namespace E_Commerce_Inerno.Core.Features.Carts.Commands.CreateCartCmd
{
    public class CreateCartHandler : IRequestHandler<CreateCartRequest, Result<bool>>
    {
        private readonly ICartRepository _cartRepo;
        private readonly IInternoEventPublisher _Publisher;
        

        public CreateCartHandler(ICartRepository cartRepo, IInternoEventPublisher Publisher)
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
            await _Publisher.PublishAuditAsync(AuditRequest);

            return Result<bool>.Success(true);
        }
    }
}
