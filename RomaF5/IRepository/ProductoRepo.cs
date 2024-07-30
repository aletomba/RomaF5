using Microsoft.EntityFrameworkCore;
using RomaF5.Data;
using RomaF5.Models;
using System.Collections;

namespace RomaF5.IRepository
{
    public class ProductoRepo : Repository<Producto>
    {
        private readonly ApplicationDbContext _context;
        public ProductoRepo(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }
        public async Task<List<Producto>> GetProducts()
        {
            return await _context.Productos.Include(x=>x.Proveedores).ToListAsync();    
        }       

    }
}
