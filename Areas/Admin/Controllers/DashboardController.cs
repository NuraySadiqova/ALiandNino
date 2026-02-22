using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AliAndNinoClone.DAL; 
namespace AliAndNinoClone.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Bazadakı ümumi sayları çəkirik
            ViewBag.TotalBooks = await _context.Books.CountAsync();
            ViewBag.TotalCategories = await _context.Categories.CountAsync();

            return View();
        }
    }
}