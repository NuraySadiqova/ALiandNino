using AliAndNinoClone.DAL;
using AliAndNinoClone.Models.Common;
using AliAndNinoClone.Models.Order;
using AliAndNinoClone.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AliAndNinoClone.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 1. Checkout (Ödəniş) səhifəsi
        public async Task<IActionResult> Checkout()
        {
            var userId = _userManager.GetUserId(User);

            // Qeyd: Basket modelində sütun adının AppUserId olduğundan əmin ol
            var basket = await _context.Baskets
                .Include(b => b.BasketItems)
                .ThenInclude(bi => bi.Book)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (basket == null || !basket.BasketItems.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            var checkoutVM = new CheckoutVM
            {
                BasketItems = basket.BasketItems,
                TotalPrice = basket.BasketItems.Sum(bi => bi.Count * bi.Book.Price)
            };

            return View(checkoutVM);
        }

        // 2. Sifarişi tamamlama (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutVM vm)
        {
            var userId = _userManager.GetUserId(User);

            var basket = await _context.Baskets
                .Include(b => b.BasketItems)
                .ThenInclude(bi => bi.Book)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (!ModelState.IsValid)
            {
                vm.BasketItems = basket?.BasketItems;
                vm.TotalPrice = basket?.BasketItems.Sum(bi => bi.Count * bi.Book.Price) ?? 0;
                return View(vm);
            }

            if (basket == null || !basket.BasketItems.Any()) return RedirectToAction("Index", "Home");

            Order newOrder = new Order
            {
                AppUserId = userId,
                Address = vm.Address,
                PhoneNumber = vm.PhoneNumber,
                OrderDate = DateTime.Now,
                TotalPrice = basket.BasketItems.Sum(bi => bi.Count * bi.Book.Price),
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in basket.BasketItems)
            {
                OrderItem orderItem = new OrderItem
                {
                    BookId = item.BookId,
                    Count = item.Count,
                    Price = item.Book.Price
                };
                newOrder.OrderItems.Add(orderItem);
            }

            _context.Orders.Add(newOrder);
            _context.BasketItems.RemoveRange(basket.BasketItems); // Səbəti təmizləyirik

            await _context.SaveChangesAsync();
            return RedirectToAction("Success", new { id = newOrder.Id });
        }

        // 3. Sifarişlərim səhifəsi
        public async Task<IActionResult> MyOrders()
        {
            var userId = _userManager.GetUserId(User);

            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .Where(o => o.AppUserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        // 4. Uğurlu sifariş səhifəsi
        public IActionResult Success(int id)
        {
            ViewBag.OrderId = id; // Bu sətir Id-ni View-a ötürür
            return View();
        }
    }
}