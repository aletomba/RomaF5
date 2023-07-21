namespace RomaF5.Models
{
	public class VentaProducto
	{
		public int VentaId { get; set; } // Clave foránea para la relación con Venta
		public Venta? Venta { get; set; } // Relación con Venta
		public int ProductoId { get; set; } // Clave foránea para la relación con Producto
		public Producto? Producto { get; set; } // Relación con Producto
		public int Cantidad { get; set; }

		// Otras propiedades y métodos relacionados con la relación entre Venta y Producto
	}
}
