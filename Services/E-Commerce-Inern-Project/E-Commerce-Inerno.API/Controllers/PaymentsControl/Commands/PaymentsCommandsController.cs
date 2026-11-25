using E_Commerce_Inerno.Core.Features.Payment.Command.CreateNewPaymentCmd;
using E_Commerce_Inerno.Core.Features.Payment.Command.InitializePaymentCmd;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.PaymentDTO;
using E_Commerce_Interno.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce_Inerno.API.Controllers.PaymentsControl.Commands
{
    [Authorize]
    [Route("api/Payments")]
    [ApiController]
    public class PaymentsCommandsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsCommandsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<Result<PaymentResponse>> AddNewPayment(PaymentRequest paymentRequest)
        {
            return await _mediator.Send(paymentRequest);
        }

        [HttpPost("[action]")]
        public async Task<Result<string>> InitializePayment(PaymentMethods paymentMethod)
        {
            var userIDStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(string.IsNullOrEmpty(userIDStr))
            {
                return Result<string>.UnAuthorized("User is not authenticated.");
            }
            var userId = Guid.Parse(userIDStr);

            return await _mediator.Send(new InitializePaymentCmd(userId, paymentMethod));
        }
    }
}
