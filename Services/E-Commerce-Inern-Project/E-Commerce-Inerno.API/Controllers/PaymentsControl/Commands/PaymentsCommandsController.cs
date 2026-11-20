using E_Commerce_Inerno.Core.Features.Payment.Command.CreateNewPaymentCmd;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.PaymentDTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("AddNewPayment")]
        public async Task<Result<PaymentResponse>> AddNewPayment(PaymentRequest paymentRequest)
        {
            return await _mediator.Send(paymentRequest);
        }
    }
}
