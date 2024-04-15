using System.ComponentModel.DataAnnotations; 

namespace DotnetCoding.Core.Models
{
    public class ProductDetails
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        [Required]
        [Range(1, 9999)]
        public int ProductPrice { get; set; }
        [Required]
        public string ProductStatus { get; set; }
        [Required]
        public DateTime RequestDate { get; set;  }
        [Required]
        public string RequestReason { get; set; }
    }
}
