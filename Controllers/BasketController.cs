using AliAndNinoClone.DAL;
using AliAndNinoClone.Models.Basket;
using AliAndNinoClone.Models.Common; // AppUser üçün lazımdır
using Microsoft.AspNetCore.Identity; // UserManager üçün
using Microsoft.AspNetCore.Authorization; // [Authorize] üçün
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AliAndNinoClone.Controllers
{
    [Authorize] // Səbətlə bağlı hər şey üçün giriş mütləqdir
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BasketController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Giriş etmiş istifadəçinin ID-sini tapmaq üçün köməkçi metod
        private string CurrentUserId => _userManager.GetUserId(User);

        public async Task<IActionResult> Index()
        {
            var basket = await _context.Baskets
                .Include(b => b.BasketItems)
                .ThenInclude(bi => bi.Book)
                .FirstOrDefaultAsync(b => b.UserId == CurrentUserId); // "1" yerinə CurrentUserId

            if (basket == null)
            {
                return View(new Basket { BasketItems = new List<BasketItem>() });
            }

            return View(basket);
        }

        public async Task<IActionResult> AddToBasket(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            var basket = await _context.Baskets
                .Include(b => b.BasketItems)
                .FirstOrDefaultAsync(b => b.UserId == CurrentUserId);

            if (basket == null)
            {
                basket = new Basket { UserId = CurrentUserId }; // Dinamik ID
                _context.Baskets.Add(basket);
                await _context.SaveChangesAsync();
            }

            var existingItem = basket.BasketItems.FirstOrDefault(bi => bi.BookId == id);

            if (existingItem != null)
            {
                existingItem.Count++;
            }
            else
            {
                basket.BasketItems.Add(new BasketItem
                {
                    BookId = id,
                    Count = 1
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> RemoveFromBasket(int id)
        {
            var basket = await _context.Baskets
                .Include(b => b.BasketItems)
                .FirstOrDefaultAsync(b => b.UserId == CurrentUserId);

            if (basket == null) return RedirectToAction("Index");

            var itemToRemove = basket.BasketItems.FirstOrDefault(bi => bi.BookId == id);

            if (itemToRemove != null)
            {
                _context.BasketItems.Remove(itemToRemove);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> IncreaseQuantity(int id)
        {
            var basket = await _context.Baskets
                .Include(b => b.BasketItems)
                .FirstOrDefaultAsync(b => b.UserId == CurrentUserId);

            if (basket == null) return NotFound();

            var item = basket.BasketItems.FirstOrDefault(i => i.BookId == id);
            if (item != null)
            {
                item.Count++;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DecreaseQuantity(int id)
        {
            var basket = await _context.Baskets
                .Include(b => b.BasketItems)
                .FirstOrDefaultAsync(b => b.UserId == CurrentUserId);

            if (basket == null) return NotFound();

            var item = basket.BasketItems.FirstOrDefault(i => i.BookId == id);
            if (item != null)
            {
                if (item.Count > 1)
                {
                    item.Count--;
                }
                else
                {
                    _context.BasketItems.Remove(item);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}