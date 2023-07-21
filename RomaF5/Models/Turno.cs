using System.ComponentModel.DataAnnotations;

namespace RomaF5.Models
{
	public class Turno
	{
		public int Id { get; set; }
        public string? Descripcion { get; set; }
        [Display(Name ="Fecha y Hora")]
		public DateTime FechaInicio { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        [Display(Name = "N° Cancha")]
        public int NumeroCancha { get; set; }
		public bool Pagado { get; set; }
		public int ClienteId { get; set; } // Clave foránea para la relación con Cliente
		public Cliente? Cliente { get; set; } // Relación con Cliente

		// Otras propiedades y métodos relacionados con la gestión de turnos

		
	}
}
