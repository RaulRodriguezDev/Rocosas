using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rocosa.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? UserApplicationId { get; set; }
        [ForeignKey("UserApplicationId")]
        public UserApplication? UserApplication { get; set; }
        public DateTime OrderDate { get; set; }
        [Required]
        public string? Phone { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required]
        public string? Email { get; set; }

    }
}
