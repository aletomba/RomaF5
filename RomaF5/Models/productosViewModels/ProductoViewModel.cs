namespace RomaF5.Models.productosViewModels
{
    public class ProductoViewModel:Producto
    {
        public bool EsMayorista { get; set; }
        public List<Producto> Productos { get; set; }
    }
}
