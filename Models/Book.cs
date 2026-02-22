using AliAndNinoClone.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace AliAndNinoClone.Models
{
    public class Book:BaseEntity
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped] 
        public IFormFile? Photo { get; set; }
        public int StockCount { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
