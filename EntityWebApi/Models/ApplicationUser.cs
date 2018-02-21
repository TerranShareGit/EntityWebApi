using Microsoft.AspNet.Identity.EntityFramework;

namespace EntityWebApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}