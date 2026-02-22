using AliAndNinoClone.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AliAndNinoClone.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookApiService _apiService;

        // Constructor vasit?sil? servisi bura daxil edirik (Dependency Injection)
        public HomeController(BookApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            // Yoxlamaq üçün "Huseyn Javid" kitablar?n? axtaraq
            var results = await _apiService.SearchBooksAsync("Huseyn Javid");

            // N?tic?ni görm?k üçün müv?qq?ti olaraq View-a gönd?ririk
            return View(results);
        }
    }
}
