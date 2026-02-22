using System.ComponentModel.DataAnnotations;

namespace AliAndNinoClone.ViewModels
{
    public class BookEditVM
    {
        public int Id { get; set; } // Redaktə üçün mütləqdir

        [Required(ErrorMessage = "Kitabın adı mütləqdir.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Müəllif adı mütləqdir.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Qiymət qeyd edilməlidir.")]
        public decimal Price { get; set; }

        public int StockCount { get; set; }

        [Required(ErrorMessage = "Kateqoriya seçilməlidir.")]
        public int CategoryId { get; set; }

        // Mövcud şəkli göstərmək və itirməmək üçün string
        public string? ExistingImageUrl { get; set; }

        // Yeni şəkil yükləmək istəyirsə (Vacib deyil - Required deyil!)
        public IFormFile? Photo { get; set; }
    }
}