using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RomaF5.Models
{
	public class Producto
	{
        public Producto()
        {
            Proveedores = new List<Proveedor>();
        }
        public int Id { get; set; }
		public string? Nombre { get; set; }
		public decimal Precio { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal PrecioMayorista { get; set; }
        public int Stock { get; set; }
		public List<VentaProducto>? VentasProductos { get; set; } // Relación muchos a muchos con VentaProducto
        public List<Proveedor>? Proveedores { get; set; }
        public string RutaImagen { get; set; }
        [NotMapped]
        public IFormFile Imagen { get; set; }             
        [NotMapped]
        public int[]? ProveedoresSeleccionados { get; set; }
        


        // Otras propiedades y métodos relacionados con el producto
        public void DescontarStock(int cantidad)
        {
            if (Stock < cantidad)
            {
                throw new InvalidOperationException("No hay stock disponible.");
            }

            Stock -= cantidad;
         
        }      

        public void CambiarPorcentaje(Producto producto)
        {
            producto.PrecioMayorista = producto.Precio + (producto.Precio * 55 / 100);
            producto.PrecioVenta = producto.PrecioMayorista + (producto.PrecioMayorista * 70 / 100);
        }
    }   

}
