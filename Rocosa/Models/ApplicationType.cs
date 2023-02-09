using System.ComponentModel.DataAnnotations;

namespace Rocosa.Models
{
    public class ApplicationType
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="The Application Name is required",AllowEmptyStrings =false)] 
        public string Name { get; set; }
    }
}
