using E_Commerce_Inerno.Core.RepositoryContracts.ICartRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IPaymentRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.PaymentDTO;
using E_Commerce_Interno.Domain.Entity;
using E_Commerce_Interno.Domain.Enums;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Payment.Command.InitializePaymentCmd
{
    public class InitializePaymentHandler : IRequestHandler<InitializePaymentCmd, Result<string>>
    {
        private readonly IPaymobService _paymobService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserRepository _userRepository;
        public InitializePaymentHandler(IPaymobService paymobService, IPaymentRepository paymentRepository, IUserRepository userRepository)
        {
            _paymobService = paymobService;
            _paymentRepository = paymentRepository;
            _userRepository = userRepository;
        }


        public async Task<Result<string>> Handle(InitializePaymentCmd request, CancellationToken cancellationToken)
        {
            var query = _userRepository.GetApplicationUserQueryByID(request.UserID);
            if (!query.Any())
            {
                return Result<string>.NotFound("User not found.");
            }

            var NewPayment = new Payments
            {
                PaymentID = Guid.NewGuid(),
                UserID = request.UserID,
                PaymentMethod = request.PaymentMethod,
                Status = StatusEnum.Pending,
                PaymentDate = DateTime.UtcNow,
                IsDeleted = false,
                Amount= query.Select(u => u.Cart.CartItems.Sum(r => (r.Product.SalePrice ?? r.Product.Price) * r.Quantity)).FirstOrDefault()
            };

            var Result = await _paymentRepository.AddNewPayment(NewPayment);
            if(!await _paymentRepository.SaveChanges())
            {
                return Result<string>.InternalError("Failed to create a new payment record.");
            }

            var initializePaymentRequest = query.Select(u => new InitializePaymentDto
            {
                TotalAmount = u.Cart.CartItems.Sum(r => (r.Product.SalePrice ?? r.Product.Price) * r.Quantity),
                PaymentMethod = request.PaymentMethod,
                FirstName = u.PersonName,
                LastName = u.PersonName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                PaymentId = NewPayment.PaymentID,
                Items = u.Cart.CartItems.Select(i => new PaymentOrderItems
                {
                    name = i.Product.ProductName,
                    amount = (int)((i.Product.SalePrice ?? i.Product.Price) * 100),
                    quantity = i.Quantity
                }).ToArray(),
                Currency = "EGP"
            }).FirstOrDefault();


            var PaymobUrl = await _paymobService.InitializePayment(initializePaymentRequest, cancellationToken);
            if (string.IsNullOrEmpty(PaymobUrl))
            {
                return Result<string>.InternalError("Failed to initialize payment.");
            }

            return Result<string>.Success(PaymobUrl);
        }
    }
}
