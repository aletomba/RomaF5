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
                entity.VentasProductos = entity.VentasProductos.Where(vp => vp.Cantidad > 0).ToList();
                await _context.Ventas.AddAsync(entity);
                await _context.AddRangeAsync(entity.VentasProductos);
                await _context.SaveChangesAsync();
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
            return await _context.Ventas
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id) ?? throw new ArgumentNullException();
        }

        public async Task Update(Venta entity)
        {
             _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
