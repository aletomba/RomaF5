using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NuGet.Protocol.Core.Types;
using RomaF5.Data;
using RomaF5.Models;

namespace RomaF5.IRepository
{
    public class ProveedorRepository : Repository<Proveedor>
    {
        private readonly ApplicationDbContext _context;
        public ProveedorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context; 
        }

        public async Task<Proveedor> GetbyId(int id)
        {
            return await _context.Proveedor.Include(p => p.ProductoProveedores).ThenInclude(x=>x.Producto).FirstOrDefaultAsync(x=>x.Id == id);      
        }

        public async Task<List<Producto>> provXprod(int id)
        {
            return await _context.Productos.Include(x => x.ProductoProveedores).ThenInclude(x => x.Proveedor).Where(x => x.Id == id).ToListAsync();
        }

        public async Task<List<Producto>> GetProductosByProveedorId(int proveedorId)
        {
            return await _context.Productos
                .Include(p => p.ProductoProveedores)
                .ThenInclude(x => x.Proveedor)
                .Where(x => x.ProductoProveedores.Any(pp => pp.ProveedorId == proveedorId))
                .Select(x => x)
                .ToListAsync();
        }
    }
}
