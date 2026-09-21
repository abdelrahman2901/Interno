using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.PaymentDTO;
using E_Commerce_Inern_Project.Core.Features.Payments.Command.CreateNewPaymentCmd;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.PaymentsControl.Commands
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
