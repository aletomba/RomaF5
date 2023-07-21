using System.Collections.Generic;

namespace RomaF5.Models
{
	public class Producto
	{
		public int Id { get; set; }
		public string Nombre { get; set; }
		public decimal Precio { get; set; }
		public int Stock { get; set; }
		public List<VentaProducto>? VentasProductos { get; set; } // Relación muchos a muchos con VentaProducto

        // Otras propiedades y métodos relacionados con el producto
        public void DescontarStock(int cantidad)
        {
            if (Stock < cantidad)
            {
                throw new InvalidOperationException("No hay suficiente stock disponible.");
            }

            Stock -= cantidad;
         
        }      
    }   

}
