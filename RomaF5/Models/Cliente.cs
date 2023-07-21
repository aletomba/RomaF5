namespace RomaF5.Models
{
	// Clase para representar un cliente
	public class Cliente
	{
		public int Id { get; set; }
		public string? Nombre { get; set; }
		public string?	 Apellido { get; set; }
		public int? NumeroCel { get; set; }
		public List<Turno>? Turnos { get; set; } // Relación uno a muchos con Turno
		public List<Venta>? Ventas { get; set; } // Relación uno a muchos con Venta

		// Otras propiedades y métodos relacionados con el cliente
	}
}
