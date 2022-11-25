using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD_Models
{
    public class OrderHeaderDTO
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Item Total")]
        public double ItemTotal { get; set; }

        [Required]
        [Display(Name = "Order Total")]
        public double OrderTotal { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        [Display(Name = "Shipping Date")]
        public DateTime ShippingDate { get; set; }

        public double ShippingCharge { get; set; }

        [Required]
        public string Status { get; set; }

        //stripe payment
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }

        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please enter near by place")]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
        public string State { get; set; }
        [Required (ErrorMessage ="Please enter your village name")]
        public string City { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string? Tracking { get; set; }
        public string? Carrier { get; set; }

    }
}
