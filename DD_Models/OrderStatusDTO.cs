using System.ComponentModel.DataAnnotations;

namespace DD_Models
{
    public class OrderStatusDTO
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
