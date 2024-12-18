using System.ComponentModel.DataAnnotations;

namespace EcommerceWebMvc.Models
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Le champ Email est obligatoire")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Le champ Mot de passe est obligatoire")]
        public string Password { get; set; } = "";

        public bool RememberMe { get; set; }
    }
}
