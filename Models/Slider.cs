using AliAndNinoClone.Models.Common;

namespace AliAndNinoClone.Models
{
    public class Slider:BaseEntity
    {
        public string ImageUrl { get; set; } 
        public string Title { get; set; }   
        public string Desc { get; set; }     
        public string RedirectUrl { get; set; }
    }
}
