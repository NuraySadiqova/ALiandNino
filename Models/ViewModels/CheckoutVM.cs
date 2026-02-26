using System.ComponentModel.DataAnnotations;
using AliAndNinoClone.Models.Basket;

namespace AliAndNinoClone.Models.ViewModels
{
    public class CheckoutVM
    {
        [Required(ErrorMessage = "Ünvan mütləqdir")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Telefon mütləqdir")]
        public string PhoneNumber { get; set; }

        public List<BasketItem>? BasketItems { get; set; }
        public decimal TotalPrice { get; set; }
    }
}