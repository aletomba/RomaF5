namespace RomaF5.Models
{
    public class Cuota
    {
        public int Id { get; set; }
        public int VentaId { get; set; }
        public Venta? Venta { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public bool Pagada { get; set; }
        // Propiedades adicionales
        public DateTime? FechaPago { get; set; } // Fecha en que se pagó la cuota
        public string? Observaciones { get; set; } // Observaciones adicionales sobre la cuota
    }
}
