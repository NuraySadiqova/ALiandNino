using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration; // Mütləq əlavə et

namespace AliAndNinoClone.Services
{
    public class GeminiApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeminiApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            // appsettings.json-dan açarı oxuyuruq
            _apiKey = configuration["GroqSettings:ApiKey"] ?? "";
        }

        public async Task<string> GetBookRecommendationAsync(string userMessage)
        {
            try
            {
                string url = "https://api.groq.com/openai/v1/chat/completions";

                var requestBody = new
                {
                    model = "llama-3.3-70b-versatile",
                    messages = new[]
     {
        new {
            role = "system",
            content = "Sən Azərbaycanın ən böyük kitab mağazası 'Ali and Nino' üçün peşəkar köməkçisən. " +
                      "QAYDALAR: " +
                      "1. Yalnız və yalnız Azərbaycan dilində cavab ver. " +
                      "2. 'Salom' yox, mütləq 'Salam' yaz. " +
                      "3. Özünü 'köməkçi ola bilərəm?' kimi deyil, 'kömək edə bilərəm?' kimi ifadə et. " +
                      "4. Cümlələrində Azərbaycan dilinin qrammatikasına (ə, ö, ğ, ü, ç, ş hərflərinə) ciddi riayət et. " +
                      "5. Başqa dillərdən (türk, özbək, tacik) sözlər qatma."+
                      "6. KItab mağazasında kömək edə bilərəm demə. "
        },
        new { role = "user", content = userMessage }
    }
                };

                var jsonRequest = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

                var response = await _httpClient.PostAsync(url, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(jsonResponse);
                return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? "Cavab tapılmadı.";
            }
            catch (Exception ex)
            {
                return $"Sistem xətası: {ex.Message}";
            }
        }
    }
}