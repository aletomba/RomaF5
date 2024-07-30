namespace RomaF5.Models
{
    public class Proveedor
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? NumeroCelular { get; set; }   
        public List<Producto> Productos { get; set; }

        public Proveedor()
        {
            Productos = new List<Producto>();
        }
    }
}
