using E_Commerce_Inerno.Core.Communication.DTOs.NotificationsDTO;
using E_Commerce_Interno.Domain.Common;

namespace E_Commerce_Inerno.Core.Communication.HttpClients
{
    public class CreateNotificationClient
    {
        private readonly HttpClient _httpClient;

        public CreateNotificationClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<bool>> CreateNotificationAsync(CreateNotificationRequest request)
        {
            HttpContent content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(request),
                System.Text.Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("api/Notifications/CreateNotification", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return Result<bool>.BadRequest(errorContent);
                }

                return Result<bool>.InternalError(errorContent);
            }

            return Result<bool>.Success(true);
        }
    }
}
