using Microsoft.AspNetCore.Identity;

namespace MVCIdentityExample.Models
{
    public class User : IdentityUser
    {
        public int BirthYear { get; set; }
    }
}
