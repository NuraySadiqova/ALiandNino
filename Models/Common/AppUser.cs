using Microsoft.AspNetCore.Identity;

namespace AliAndNinoClone.Models.Common
{
    // IdentityUser onsuz da daxilində Email, UserName, PasswordHash saxlayır.
    // Biz bura sadəcə istifadəçinin Ad və Soyadını (FullName) əlavə edirik.
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}