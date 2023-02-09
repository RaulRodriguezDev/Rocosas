using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rocosa.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string Name  { get; set; }

        public int ShowOrder { get; set; }
    }
}
