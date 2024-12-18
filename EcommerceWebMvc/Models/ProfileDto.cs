using System.ComponentModel.DataAnnotations;

namespace EcommerceWebMvc.Models
{
	public class ProfileDto
	{
		[Required(ErrorMessage = "Le champ Prénom est obligatoire"), MaxLength(100)]
		public string FirstName { get; set; } = "";

		[Required(ErrorMessage = "Le champ Nom de famille est obligatoire"), MaxLength(100)]
		public string LastName { get; set; } = "";

		[Required, EmailAddress, MaxLength(100)]
		public string Email { get; set; } = "";

		[Phone(ErrorMessage = "Le format du numéro de téléphone n'est pas valide"), MaxLength(20)]
		public string? PhoneNumber { get; set; }

		[Required, MaxLength(200)]
		public string Address { get; set; } = "";



	}
}
