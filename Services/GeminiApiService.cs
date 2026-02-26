using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace AliAndNinoClone.Services
{
    public class GeminiApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        // Söhbət tarixçəsini yadda saxlamaq üçün statik siyahı
        // QEYD: Bu sadə versiyadır, bütün istifadəçilər eyni tarixçəni görəcək. 
        // Real layihədə bunu Session və ya Verilənlər Bazası ilə fərdiləşdirmək lazımdır.
        private static readonly List<object> _chatHistory = new List<object>
        {
            new {
                role = "system",
                content = "Sən Azərbaycanın 'Ali and Nino' kitab mağazası üçün peşəkar köməkçisən. " +
                          "QAYDALAR: " +
                          "1. Yalnız Azərbaycan dilində cavab ver. " +
                          "2. Hər mesajda yenidən 'Salam' yazma! Əgər söhbət artıq başlayıbsa, birbaşa suala keç. " +
                          "3. 'Salom' qətiyyən yazma, yalnız 'Salam' istifadə et. " +
                          "4. 'Kitab mağazasında kömək edə bilərəm' kimi cümlələri hər mesajda təkrar etmə. " +
                          "5. Ə, ö, ğ, ü, ç, ş hərflərindən düzgün istifadə et. " +
                          "6. Qısa, səmimi və köməkçi ol."
            }
        };

        public GeminiApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["GroqSettings:ApiKey"] ?? "";
        }

        public async Task<string> GetBookRecommendationAsync(string userMessage)
        {
            try
            {
                string url = "https://api.groq.com/openai/v1/chat/completions";

                // 1. İstifadəçinin yeni mesajını tarixçəyə əlavə edirik
                _chatHistory.Add(new { role = "user", content = userMessage });

                var requestBody = new
                {
                    model = "llama-3.3-70b-versatile",
                    messages = _chatHistory.ToArray() // Tarixçəni bütöv göndəririk
                };

                var jsonRequest = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

                var response = await _httpClient.PostAsync(url, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(jsonResponse);
                var aiContent = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? "Bağışlayın, cavab verə bilmirəm.";

                // 2. AI-ın cavabını da tarixçəyə əlavə edirik ki, növbəti mesajda xatırlasın
                _chatHistory.Add(new { role = "assistant", content = aiContent });

                // Tarixçənin çox böyüyüb limiti keçməməsi üçün son 10-15 mesajı saxlamaq olar
                if (_chatHistory.Count > 20)
                {
                    _chatHistory.RemoveAt(1); // Sistem mesajını yox, ondan sonrakı köhnə mesajı silirik
                }

                return aiContent;
            }
            catch (Exception ex)
            {
                return $"Sistem xətası: {ex.Message}";
            }
        }
    }
}