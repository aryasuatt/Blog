using Microsoft.AspNetCore.Identity;
using BlogSite.Models;
namespace BlogSite.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
