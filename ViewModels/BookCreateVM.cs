using System.ComponentModel.DataAnnotations;

namespace AliAndNinoClone.ViewModels
{
    public class BookCreateVM
    {
        [Required(ErrorMessage = "Kitabın adı mütləq daxil edilməlidir.")]
        [StringLength(100, ErrorMessage = "Kitab adı 100 simvoldan çox ola bilməz.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Müəllif adı mütləqdir.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Qiymət qeyd edilməlidir.")]
        [Range(0.01, 10000, ErrorMessage = "Qiymət 0.01 ilə 10,000 arasında olmalıdır.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Kateqoriya seçilməlidir.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Kitabın şəkli mütləq yüklənməlidir.")]
        public IFormFile Photo { get; set; } // Şəkli fayl olaraq qəbul edirik
        [Required(ErrorMessage = "Stok sayını daxil edin.")]
        [Range(0, 10000, ErrorMessage = "Stok sayı mənfi ola bilməz.")]
        public int StockCount { get; set; }
    }
}
