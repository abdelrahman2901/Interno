using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;
using E_Commerce_Interno.Domain.Common;

namespace E_Commerce_Inerno.Core.Communication.HttpClients
{
    public class CreateAuditClient
    {
        private readonly HttpClient httpClient;
        public CreateAuditClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Result<bool>> CreateAuditLogAsync(AuditRequest request)
        {
            HttpContent content = new StringContent(System.Text.Json.JsonSerializer.Serialize(request), System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/Audit/CreateAudit", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine($"Bad Request: {errorContent}");

                    return Result<bool>.BadRequest(errorContent);
                }


                Console.WriteLine($"Failed to create audit log. Status code: {response.StatusCode}");
                return Result<bool>.InternalError(errorContent);
            }

            Console.WriteLine("Audit log created successfully.");
            return Result<bool>.Success(true);
        }

    }
}
