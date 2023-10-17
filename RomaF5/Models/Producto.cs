using System.Collections.Generic;

namespace RomaF5.Models
{
	public class Producto
	{
		public int Id { get; set; }
		public string? Nombre { get; set; }
		public float Precio { get; set; }
		public float? Stock { get; set; }
		public List<VentaProducto>? VentasProductos { get; set; } // Relación muchos a muchos con VentaProducto

        // Otras propiedades y métodos relacionados con el producto
        public void DescontarStock(float? cantidad)
        {
            if (Stock < cantidad)
            {
                throw new InvalidOperationException("No hay suficiente stock disponible.");
            }

            Stock -= cantidad;
         
        }      
    }   

}
