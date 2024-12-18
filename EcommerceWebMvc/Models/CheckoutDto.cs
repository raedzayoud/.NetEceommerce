using System.ComponentModel.DataAnnotations;

namespace EcommerceWebMvc.Models
{
    public class CheckoutDto
    {
        [Required(ErrorMessage = "L'adresse de livraison est obligatoire.")]
        [MaxLength(200)]
        public string DeliveryAddress { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
    }
}
