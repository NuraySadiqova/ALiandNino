using AliAndNinoClone.Models.Common;

namespace AliAndNinoClone.Models.Basket
{
    public class BasketItem:BaseEntity
    {
        public int BookId { get; set; }
      //  public Book Book { get; set; }
        public int BasketId { get; set; }
        public int Count { get; set; }
    }
}
