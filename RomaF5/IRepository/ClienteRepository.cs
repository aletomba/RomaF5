using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RomaF5.Data;
using RomaF5.Models;

namespace RomaF5.IRepository
{
    public class ClienteRepository : Repository<Cliente>
    {
        private readonly ApplicationDbContext _context;
        public ClienteRepository(ApplicationDbContext context) : base(context)
        {
            _context = context; 
        } 

        public async Task<List<Cliente>> GetAllClient()
        {
            return await _context.Clientes.Include(x=>x.Ventas).ToListAsync(); 
        }
    }
}
