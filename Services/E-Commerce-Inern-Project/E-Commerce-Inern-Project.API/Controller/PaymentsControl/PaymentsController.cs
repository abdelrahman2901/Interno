using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.PaymentDTO;
using E_Commerce_Inern_Project.Core.Features.Payments.Command.CreateNewPaymentCmd;
//using E_Commerce_Inern_Project.Core.Features.Payments.Command.DeleteLastPaymentCmd;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controller.PaymentsControl
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("AddNewPayment")]
        public async Task<Result<PaymentResponse>> AddNewPayment( PaymentRequest paymentRequest)
        {
            return await _mediator.Send(paymentRequest);
        }

        //[HttpDelete("DeleteLastPayment")]
        //public async Task<Result<bool>> DeleteLastPayment()
        //{
        //    return await _mediator.Send(new DeleteLastPaymentRequest());
        //}

    }
}
