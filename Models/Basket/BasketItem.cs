using AliAndNinoClone.Models.Common;

namespace AliAndNinoClone.Models.Basket
{
    public class BasketItem : BaseEntity
    {
        public int BookId { get; set; }

        // Bu sətiri aktiv etmək vacibdir ki, 
        // səbətdə kitabın detallarını (ad, qiymət) göstərə bilək.
        public Book Book { get; set; }

        public int BasketId { get; set; }
        public Basket Basket { get; set; } // Səbətin özü ilə əlaqə

        public int Count { get; set; }
    }
}