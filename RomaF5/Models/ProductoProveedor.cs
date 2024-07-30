namespace RomaF5.Models
{
    public class ProductoProveedor
    {
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }
        public int ProveedorId { get; set; }
        public Proveedor? Proveedor { get; set; }
        public decimal? Precio { get; set; } // Precio del producto para este proveedor
    }
}
