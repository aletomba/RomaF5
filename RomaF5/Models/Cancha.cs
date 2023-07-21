namespace RomaF5.Models
{
	public class Cancha
	{
		public int Id { get; set; }
		public string? Nombre { get; set; }
		public bool Disponible { get; set; }
		public List<Turno>? Turnos { get; set; } // Relación uno a muchos con Turno

		// Otras propiedades y métodos relacionados con la cancha
	}
}
