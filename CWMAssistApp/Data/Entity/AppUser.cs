using Microsoft.AspNetCore.Identity;

namespace CWMAssistApp.Data.Entity
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public Guid CompanyId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
