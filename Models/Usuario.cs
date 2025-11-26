using System.ComponentModel.DataAnnotations;
namespace ulp_lab3_inmobiliaria_servidor.Models
{
	public class Usuario
	{
		[Key]
		public int Id { get; set; }
		public string Nombre { get; set; } = "";
		public string Apellido { get; set; } = "";
		public long Dni { get; set; }
		public string? Telefono { get; set; }
		public string Email { get; set; } = "";
		public string Password { get; set; } = "";
		public string Avatar { get; set; } = "";
		public int Estado { get; set; } = 1;

		public Usuario() { }

		public Usuario(Usuario usuario)
		{
			Id = usuario.Id;
			Nombre = usuario.Nombre;
			Apellido = usuario.Apellido;
			Dni = usuario.Dni;
			Telefono = usuario.Telefono;
			Email = usuario.Email;
			Password = usuario.Password;
			Avatar = usuario.Avatar;
			Estado = usuario.Estado;
		}

	}
}