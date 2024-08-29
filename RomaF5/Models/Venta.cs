using System.ComponentModel.DataAnnotations;

namespace RomaF5.Models
{
    public class Venta
    {
        public Venta()
        {
            VentasProductos = new List<VentaProducto>();
            Cuotas = new List<Cuota>();
        }
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Today;
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        //public decimal PrecioCosto { get; set; }
        public List<VentaProducto>? VentasProductos { get; set; }
        public decimal? Total { get; set; }
        public TipoPago? TipoPago { get; set; } 
        public List<Cuota>? Cuotas { get; set; }
        public decimal? MontoPagado { get; set; }     

        // Propiedades adicionales
        public bool? Pagado { get; set; } // Indica si la venta está completamente pagada
        public DateTime? FechaPago { get; set; } // Fecha en que se pagó la venta
        public string? Observaciones { get; set; } // Observaciones adicionales sobre la venta
        public int? NumeroCuotas { get; set; } = 0;// Número de cuotas para pago en cuota
        public int? CuotasPagas { get; set; }
    }
    public enum TipoPago
    {
        Efectivo,
        Debito,
        Transferencia,
        Cuotas
    }

}
