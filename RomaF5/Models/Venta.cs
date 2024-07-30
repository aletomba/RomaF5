namespace RomaF5.Models
{
	public class Venta
	{
        public Venta()
        {
			VentasProductos = new List<VentaProducto>();
		}
        public int Id { get; set; }
		public DateTime Fecha { get; set; }
		public int ClienteId { get; set; }
		public Cliente? Cliente { get; set; }
        //public decimal PrecioCosto { get; set; }
        public List<VentaProducto>? VentasProductos { get; set; }
		public decimal? Total { get;set; }

	
	}

}
