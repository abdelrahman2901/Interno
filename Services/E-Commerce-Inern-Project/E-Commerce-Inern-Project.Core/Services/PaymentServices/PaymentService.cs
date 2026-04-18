using AutoMapper;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IPaymentRepo;
using E_Commerce_Inern_Project.Core.DTO.PaymentDTO;
using E_Commerce_Inern_Project.Core.Features.Payments.Command.CreateNewPaymentCmd;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.ServicesContracts.IPaymentServices;
using System.Text.Json;
namespace E_Commerce_Inern_Project.Core.Services.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _PaymentRepo;
        private readonly IMapper _Mapper;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly string _AuditRoutingKey = "Interno.Audit";
        public PaymentService(IPaymentRepository PaymentRepo, IRabbitMQPublisher Publisher, IMapper Mapper)
        {
            _PaymentRepo = PaymentRepo;
            _Publisher = Publisher;
            _Mapper = Mapper;
        }

        public async Task<Result<PaymentResponse>> AddNewPayment(PaymentRequest paymentRequest)
        {
            Payments newPayment = _Mapper.Map<Payments>(paymentRequest);
            if (newPayment.PaymentMethod == Enums.PaymentMethods.CreditCard)
            {
                newPayment.Status = Enums.StatusEnum.Approved;
            }
            Payments? NewPayment = await _PaymentRepo.AddNewPayment(newPayment);

            if (NewPayment == null)
            {
                return Result<PaymentResponse>.InternalError("Failed to add new payment.");
            }
            if (!await _PaymentRepo.SaveChanges())
            {
                return Result<PaymentResponse>.InternalError("Failed To Save Changes.");
            }
            string JsonNewValues = JsonSerializer.Serialize(NewPayment);
            AuditRequest AuditRequest = new(paymentRequest.UserID, ActionTypeEnum.Create, nameof(Payments), null, JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            return Result<PaymentResponse>.Success(_Mapper.Map<PaymentResponse>(NewPayment));
        }

        


    }
}
