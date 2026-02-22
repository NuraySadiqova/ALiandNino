using AliAndNinoClone.DAL;
using AliAndNinoClone.Models.Basket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AliAndNinoClone.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }

        // Səbətə məhsul əlavə etmək üçün Action
        public async Task<IActionResult> AddToBasket(int id)
        {
            // 1. Kitabın bazada olub-olmadığını yoxla
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            // 2. İstifadəçinin səbətini tap (Hələlik login sistemi yoxdursa, test üçün UserId = "1" götürürük)
            var basket = await _context.Baskets
                .Include(b => b.BasketItems)
                .FirstOrDefaultAsync(b => b.UserId == "1");

            if (basket == null)
            {
                basket = new Basket { UserId = "1" };
                _context.Baskets.Add(basket);
            }

            // 3. Bu kitab artıq səbətdə varmı?
            var existingItem = basket.BasketItems.FirstOrDefault(bi => bi.BookId == id);

            if (existingItem != null)
            {
                // Varsa sayını artır
                existingItem.Count++;
            }
            else
            {
                // Yoxdursa yeni sətir əlavə et
                basket.BasketItems.Add(new BasketItem
                {
                    BookId = id,
                    Count = 1
                });
            }

            await _context.SaveChangesAsync();

            // Səbətə əlavə etdikdən sonra gəldiyi səhifəyə qaytar
            return RedirectToAction("Index", "Home");
        }
    }
}