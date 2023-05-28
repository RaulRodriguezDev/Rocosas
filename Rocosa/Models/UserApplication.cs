using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rocosa.Models
{
    public class UserApplication : IdentityUser
    {
        public string FullName { get; set; }
        [NotMapped]
        public string Address { get; set; }
        [NotMapped]
        public string City { get; set; }
    }
}
