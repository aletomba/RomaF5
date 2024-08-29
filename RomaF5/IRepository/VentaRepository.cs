using Microsoft.EntityFrameworkCore;
using RomaF5.Data;
using RomaF5.Models;

namespace RomaF5.IRepository
{
    public class VentaRepository : IRepository<Venta>
    {
        private readonly ApplicationDbContext _context;

        public VentaRepository(ApplicationDbContext context)
        {
            _context = context;      
        }
        public async Task AddAsync(Venta entity)
        {
            if(entity != null)
            {
                if (entity.NumeroCuotas == 0) 
                {
                    entity.VentasProductos = entity.VentasProductos.Where(vp => vp.Cantidad > 0).ToList();
                    await _context.Ventas.AddAsync(entity);
                    await _context.AddRangeAsync(entity.VentasProductos);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    entity.VentasProductos = entity.VentasProductos.Where(vp => vp.Cantidad > 0).ToList();
                    await _context.Ventas.AddAsync(entity);
                    await _context.AddRangeAsync(entity.VentasProductos);
                    await _context.AddRangeAsync(entity.Cuotas);
                    await _context.SaveChangesAsync();
                }
               
            }
        }

        public async Task Delete(Venta entity)
        {
            _context.Ventas.Remove(entity);
            await _context.SaveChangesAsync();  
        }

        public async Task<List<Venta>> GetAllAsync()
        {
            return await _context.Ventas.Include(v => v.Cliente).
                ThenInclude(x => x.Turnos).Include(x => x.VentasProductos).
                ThenInclude(x => x.Producto).ToListAsync() ?? throw new ArgumentNullException();           
        }

        public async Task<Venta> GetByIdAsync(int? id)
        {
            return await _context.Ventas.Include(x=>x.Cuotas).Include(x=>x.VentasProductos).ThenInclude(x=>x.Producto)
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id) ?? throw new ArgumentNullException();
        }

        public async Task Update(Venta entity)
        {
             _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Venta>> GetByDate(DateTime fecha)
        {
           var fechas = await _context.Ventas.Include(x=>x.Cliente)
                                             .Include(x => x.VentasProductos)
                                             .ThenInclude(x => x.Producto)
                                             .Where(x => x.Fecha.Date == fecha.Date).ToListAsync();

            return fechas;  
        }
        public async Task<int> PagarCuota(int id, decimal monto)
        {
            var venta = await _context.Ventas
                                .Include(v => v.Cuotas)
                                .Include(v => v.VentasProductos)
                                .ThenInclude(vp => vp.Producto)
                                .FirstOrDefaultAsync(v => v.Id == id);
        
            if (venta != null)
            {

                var cuota = new Cuota { Monto = monto, FechaPago = DateTime.Now };                
               
                venta.Cuotas.Add(cuota);

                venta.MontoPagado = 0;
                foreach(var c in venta.Cuotas)
                {
                    venta.MontoPagado += c.Monto;
                }

                if(venta.NumeroCuotas == venta.Cuotas.Count())
                {
                    foreach (var producto in venta.VentasProductos)
                    {
                        var prod = await _context.Productos.FindAsync(producto.ProductoId);
                        if (prod != null && producto.Cantidad > 0)
                        {
                            prod.DescontarStock(producto.Cantidad);
                        }
                    }
                    venta.TipoPago = TipoPago.Efectivo;
                    venta.NumeroCuotas = 0;
                    await _context.SaveChangesAsync();
                    return 1;
                }
                await _context.SaveChangesAsync();
            }
           
            return 3;
        }
    }
}
