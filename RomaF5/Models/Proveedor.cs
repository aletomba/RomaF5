namespace RomaF5.Models
{
    public class Proveedor
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? NumeroCelular { get; set; }   
        public List<ProductoProveedor> ProductoProveedores { get; set; }

        public Proveedor()
        {
            ProductoProveedores = new List<ProductoProveedor>();
        }
    }
}
