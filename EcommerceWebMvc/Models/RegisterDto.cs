using System.ComponentModel.DataAnnotations;

namespace EcommerceWebMvc.Models
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Le champ Prénom est obligatoire"), MaxLength(100)]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Le champ Nom de famille est obligatoire"), MaxLength(100)]
        public string LastName { get; set; } = "";

        [Required(ErrorMessage = "Le champ Email est obligatoire"), EmailAddress(ErrorMessage = "Veuillez entrer une adresse e-mail valide"), MaxLength(100)]
        public string Email { get; set; } = "";

        [Phone(ErrorMessage = "Le format du numéro de téléphone n'est pas valide"), MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Le champ Adresse est obligatoire"), MaxLength(200)]
        public string Address { get; set; } = "";

        [Required(ErrorMessage = "Le champ Mot de passe est obligatoire"), MaxLength(100)]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Le champ Confirmer le mot de passe est obligatoire")]
        [Compare("Password", ErrorMessage = "Le mot de passe et la confirmation ne correspondent pas")]
        public string ConfirmPassword { get; set; } = "";
    }
}
