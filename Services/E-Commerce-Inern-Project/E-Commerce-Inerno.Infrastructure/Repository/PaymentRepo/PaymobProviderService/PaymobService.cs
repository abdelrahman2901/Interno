using E_Commerce_Inerno.Core.RepositoryContracts.IPaymentRepo;
using E_Commerce_Interno.Domain.DTO.PaymentDTO;
using E_Commerce_Interno.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace E_Commerce_Inerno.Infrastructure.Repository.PaymentRepo.PaymobProviderService
{
    /// <summary>
    /// Integrates with Paymob to create payment intentions and return unified checkout URLs.
    /// </summary>
    public class PaymobService : IPaymobService
    {
        private readonly PaymobHttpClient _paymobHttpClient;
        private readonly IConfiguration _config;
        private readonly ILogger<PaymobService> _logger;

        public PaymobService(PaymobHttpClient paymobHttpClient, IConfiguration config, ILogger<PaymobService> logger)
        {
            _paymobHttpClient = paymobHttpClient;
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Creates a Paymob payment intention and returns the unified checkout URL.
        /// </summary>
        /// <param name="initializePaymentDto">DTO containing payment initialization details.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>Checkout URL on success; null if the Paymob call fails.</returns>
        public async Task<string?> InitializePayment(
           InitializePaymentDto initializePaymentDto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var secretKey = _config["Paymob:SecretKey"];
                var PublicKey = _config["Paymob:PublicKey"];

                var integrationId = GetIntegerationID(initializePaymentDto.PaymentMethod);
                var BaseUrl = _config["Paymob:BaseUrl"];
                _paymobHttpClient.SetBearerToken(secretKey ?? string.Empty);

                var body = new
                {
                    amount = (int)(initializePaymentDto.TotalAmount * 100), // Paymob uses cents/piasters
                    currency = initializePaymentDto.Currency, // "EGP"
                    payment_methods = new[] { integrationId },
                    special_reference = initializePaymentDto.PaymentId.ToString(),
                    items = initializePaymentDto.Items,
                    billing_data = new
                    {
                        first_name = initializePaymentDto.FirstName,
                        last_name = initializePaymentDto.LastName,
                        phone_number = initializePaymentDto.PhoneNumber,
                        email = initializePaymentDto.Email
                    }
                };

                var response = await _paymobHttpClient.PostAsJsonAsync(
                    $"{BaseUrl}/v1/intention/", body, cancellationToken);

                var result = await _paymobHttpClient.ReadFromJsonAsync<JsonElement>(response.Content, cancellationToken);
                var rawResponse = await _paymobHttpClient.ReadAsStringAsync(response.Content, cancellationToken);
                _logger.LogInformation("Paymob intention status={StatusCode} response={Response}", response.StatusCode, rawResponse);
                string clientSecret = result.GetProperty("client_secret").GetString()!;
                string PaymobUrl = $"https://accept.paymob.com/unifiedcheckout/?publicKey={PublicKey}&clientSecret={clientSecret}";
                return PaymobUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Paymob payment intention");
                return null;
            }
        }

        private int GetIntegerationID(PaymentMethods PaymentMethod)
        {
            var IntegerationIDsSection = _config.GetSection("Paymob:IntegrationIDs");
            return PaymentMethod switch
            {
                PaymentMethods.CreditCard => int.Parse(IntegerationIDsSection["OnlineCard"]!),
                PaymentMethods.Wallet => int.Parse(IntegerationIDsSection["Wallet"]!),
                _ => throw new ArgumentOutOfRangeException(nameof(PaymentMethod), $"Not expected payment method value: {PaymentMethod}"),
            };
        }
    }
}
