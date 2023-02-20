using Microsoft.AspNetCore.Identity;

namespace Rocosa.Models
{
    public class UserApplication : IdentityUser
    {
        public string FullName { get; set; }
    }
}
