using AliAndNinoClone.DAL;
using AliAndNinoClone.Models.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AliAndNinoClone.ViewComponents
{
    public class BasketCountViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BasketCountViewComponent(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int count = 0;

            // İstifadəçi giriş edibsə, onun səbətindəki məhsul sayını hesabla
            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(UserClaimsPrincipal);
                var basket = await _context.Baskets
                    .Include(b => b.BasketItems)
                    .FirstOrDefaultAsync(b => b.UserId == userId);

                if (basket != null)
                {
                    count = basket.BasketItems.Sum(bi => bi.Count);
                }
            }

            return View(count); // Sayı View-a göndər
        }
    }
}