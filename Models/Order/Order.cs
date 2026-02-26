using AliAndNinoClone.Models.Common; // AppUser-i tanıması üçün
using System.ComponentModel.DataAnnotations;

namespace AliAndNinoClone.Models.Order
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalPrice { get; set; }

        // İstifadəçi məlumatları
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        // Çatdırılma məlumatları
        [Required(ErrorMessage = "Ünvan mütləqdir"), MaxLength(250)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Telefon nömrəsi mütləqdir"), MaxLength(50)]
        public string PhoneNumber { get; set; }

        // Sifarişin içindəki məhsullar
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}