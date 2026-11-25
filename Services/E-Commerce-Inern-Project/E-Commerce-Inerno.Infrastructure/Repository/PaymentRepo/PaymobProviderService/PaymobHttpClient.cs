using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace E_Commerce_Inerno.Infrastructure.Repository.PaymentRepo.PaymobProviderService
{
    /// <summary>
    /// Typed HTTP client for Paymob API calls.
    /// </summary>
    public class PaymobHttpClient
    {
        private readonly HttpClient _httpClient;

        public PaymobHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void SetBearerToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Token", token);
        }

        public Task<HttpResponseMessage> PostAsJsonAsync<T>(
            string requestUri,
            T value,
            CancellationToken cancellationToken = default) =>
            _httpClient.PostAsJsonAsync(requestUri, value, cancellationToken);

        public Task<T?> ReadFromJsonAsync<T>(
            HttpContent content,
            CancellationToken cancellationToken = default) =>
            content.ReadFromJsonAsync<T>(cancellationToken);

        public Task<string> ReadAsStringAsync(
            HttpContent content,
            CancellationToken cancellationToken = default) =>
            content.ReadAsStringAsync(cancellationToken);
    }
}
