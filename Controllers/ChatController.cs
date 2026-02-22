using AliAndNinoClone.Services; // BU SƏTİR MÜTLƏQDİR
using Microsoft.AspNetCore.Mvc;

namespace AliAndNinoClone.Controllers
{
    public class ChatController : Controller
    {
        private readonly GeminiApiService _geminiService;

        public ChatController(GeminiApiService geminiService)
        {
            _geminiService = geminiService;
        }

        [HttpPost]
        public async Task<IActionResult> Ask(string message)
        {
            if (string.IsNullOrEmpty(message))
                return Json(new { answer = "Zəhmət olmasa mesaj yazın." });

            try
            {
                var response = await _geminiService.GetBookRecommendationAsync(message);
                return Json(new { answer = response });
            }
            catch (Exception ex)
            {
                return Json(new { answer = "Sistem xətası: " + ex.Message });
            }
        }
    }
}