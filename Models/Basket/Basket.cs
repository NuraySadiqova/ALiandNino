using AliAndNinoClone.Models.Common;
using System.Collections.Generic; // List üçün lazımdır

namespace AliAndNinoClone.Models.Basket
{
    public class Basket : BaseEntity
    {
        // UserId-ni mütləq string saxla (Identity-də Id string olur)
        public string UserId { get; set; }

        // Navigation Property: Səbətin içindəki məhsulların siyahısı
        public List<BasketItem> BasketItems { get; set; } = new();
    }
}