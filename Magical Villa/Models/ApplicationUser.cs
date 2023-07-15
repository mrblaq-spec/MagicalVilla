using Microsoft.AspNetCore.Identity;

namespace MagicalVilla_API.Models
{
    public class ApplicationUser : IdentityUser
    {
        /*public int Id { get; set; }
        public string UserName { get; set; }*/
        public string Name { get; set; }
    }
}
