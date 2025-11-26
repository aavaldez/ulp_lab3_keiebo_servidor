using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ulp_lab3_inmobiliaria_servidor.Models
{
	public class Tarea
	{
		[Key]
		public int Id { get; set; }
		public int Numero { get; set; } = 0;
		[DataType(DataType.Date)]
		public DateTime Fecha { get; set; } = DateTime.Today;
		public Boolean Estado { get; set; } = true;
		[Display(Name = "Reunion")]
		public int ReunionId { get; set; }
		[ForeignKey(nameof(ReunionId))]
		public Reunion? Reunion { get; set; }
	}
}
