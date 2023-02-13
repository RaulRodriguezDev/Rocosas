

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rocosa.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="The product name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage ="The short description musn't be empty")]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage ="The product price is required")]
        [Range(1,double.MaxValue,ErrorMessage ="The price must be major than 0")]
        public double ProductPrice { get; set; }

        public string ImageUrl { get; set; }

        // Foreign Key
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public int ApplicationTypeId { get; set; }

        [ForeignKey("ApplicationTypeId")]
        public virtual ApplicationType ApplicationType { get; set; }

    }
}
