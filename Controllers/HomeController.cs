using AliAndNinoClone.DAL; // AppDbContext üçün lazımdır
using AliAndNinoClone.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AliAndNinoClone.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookApiService _apiService;
        private readonly AppDbContext _context; // 1. Context-i buraya əlavə etdik

        // 2. Constructor-da həm servisi, həm də context-i qəbul edirik
        public HomeController(BookApiService apiService, AppDbContext context)
        {
            _apiService = apiService;
            _context = context; // 3. Context-i mənimsətdik
        }

        public async Task<IActionResult> Index()
        {
            // İndi _context artıq tanınır və bazadan kitabları çəkə bilər
            var books = await _context.Books.ToListAsync();

            // Səhifəyə bazadakı kitabların siyahısını göndəririk
            return View(books);
        }
    }
}