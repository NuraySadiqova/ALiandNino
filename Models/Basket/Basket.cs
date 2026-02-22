using AliAndNinoClone.Models.Common;

namespace AliAndNinoClone.Models.Basket
{
    public class Basket:BaseEntity
    {
        public string UserId { get; set; }
        public List<BasketItem> BasketItems { get; set; } = new();
    }
}
