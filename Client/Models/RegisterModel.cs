using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
	public class RegisterModel
	{
		[Required(ErrorMessage = "Email nie moze byc pusty"), EmailAddress(ErrorMessage = "Pole email powinno byc poprawnym mailem")]
		public string Email { get; set; } = string.Empty;

		[
			Required(ErrorMessage = "Haslo nie moze byc puste"),
			RegularExpression
			(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$",
			ErrorMessage = "Hasło musi miec co najmniej: 6 znakow , jedna duza litere , jedna małą litere , i cyfre"
			)]

		public string Password { get; set; } = string.Empty;

		[Compare(nameof(Password) , ErrorMessage = "Hasła muszą byc takie same")]
		public string RepeatPassword {  get; set; } = string.Empty;
	}
}
