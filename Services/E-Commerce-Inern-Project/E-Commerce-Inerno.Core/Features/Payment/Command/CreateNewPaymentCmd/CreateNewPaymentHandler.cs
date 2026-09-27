using AutoMapper;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IPaymentRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.PaymentDTO;
using E_Commerce_Interno.Domain.Entity;
using E_Commerce_Interno.Domain.Enums;
using MediatR;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.Payment.Command.CreateNewPaymentCmd
{
    public class CreateNewPaymentHandler : IRequestHandler<PaymentRequest, Result<PaymentResponse>>
    {
        private readonly IPaymentRepository _PaymentRepo;
        private readonly IMapper _Mapper;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly string _AuditRoutingKey = "Interno.Audit";

        public CreateNewPaymentHandler(IPaymentRepository PaymentRepo, IRabbitMQPublisher Publisher, IMapper Mapper)
        {
            _PaymentRepo = PaymentRepo;
            _Publisher = Publisher;
            _Mapper = Mapper;
        }

        public async Task<Result<PaymentResponse>> Handle(PaymentRequest request, CancellationToken cancellationToken)
        {
            Payments newPayment = _Mapper.Map<Payments>(request);
            if (request.PaymentMethod == PaymentMethods.CreditCard)
            {
                newPayment.Status = StatusEnum.Approved;
            }
            bool result = await _PaymentRepo.AddNewPayment(newPayment);

            if (!result)
            {
                return Result<PaymentResponse>.InternalError("Failed to add new payment.");
            }
            if (!await _PaymentRepo.SaveChanges())
            {
                return Result<PaymentResponse>.InternalError("Failed To Save Changes.");
            }
            string JsonNewValues = JsonSerializer.Serialize(newPayment);
            AuditRequest AuditRequest = new(request.UserID, ActionTypeEnum.Create, nameof(Payments), null, JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            return Result<PaymentResponse>.Success(_Mapper.Map<PaymentResponse>(newPayment));
        }
    }
}
