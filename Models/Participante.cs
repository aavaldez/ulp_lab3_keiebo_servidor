using System.ComponentModel.DataAnnotations;
namespace ulp_lab3_inmobiliaria_servidor.Models
{
	public class Participante
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

		public Participante() { }

		public Participante(Participante participante)
		{
			Id = participante.Id;
			Nombre = participante.Nombre;
			Apellido = participante.Apellido;
			Dni = participante.Dni;
			Telefono = participante.Telefono;
			Email = participante.Email;
			Password = participante.Password;
			Avatar = participante.Avatar;
			Estado = participante.Estado;
		}

	}
}