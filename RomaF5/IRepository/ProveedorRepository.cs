using Microsoft.EntityFrameworkCore;
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
            return await _context.Proveedor.Include(p => p.Productos).FirstOrDefaultAsync(X=>X.Id == id);      
        }
       
    }
}
