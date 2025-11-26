using System.ComponentModel.DataAnnotations;

namespace ulp_lab3_inmobiliaria_servidor.Models
{
	public class LoginView
	{
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}