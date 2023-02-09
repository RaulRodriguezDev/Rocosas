using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Versioning;

namespace Rocosa.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="The name is required")]
        public string Name  { get; set; }

        [Required(ErrorMessage ="The order field is required"]
        [Range(1,int.MaxValue,ErrorMessage ="The order must be greater than 0")]
        public int ShowOrder { get; set; }
    }
}
