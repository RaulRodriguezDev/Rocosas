
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rocosa.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }

        public string CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public UserApplication UserApplication { get; set; }

        [Required]
        public DateTime SaleDate { get; set; }

        public DateTime ShipmentDate { get; set; }

        [Required]
        public double FinalTotalSale { get; set; }

        public string SaleStatus { get; set; }

        public DateTime PayingDate { get; set; }

        public string TransactionId { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string FullName { get; set; }

        public string Email { get; set; }

    }
}
