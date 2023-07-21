using Microsoft.EntityFrameworkCore;
using RomaF5.Data;
using RomaF5.Models;

namespace RomaF5.IRepository
{
    public class TurnoRepository : IRepository<Turno>
    {
        private readonly ApplicationDbContext _context;

        public TurnoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Turno entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Turno entity)
        {
          _context.Turnos.Remove(entity);
           await _context.SaveChangesAsync();
        }

        public async Task<List<Turno>> GetAllAsync()
        {
            return await _context.Turnos.Include(t => t.Cliente).ToListAsync();
        }

        public async Task<Turno> GetByIdAsync(int? id)
        {
            return await _context.Turnos
            .Include(t => t.Cliente)
            .FirstOrDefaultAsync(m => m.Id == id) ?? throw new ArgumentNullException();
            
        }

        public async Task Update(Turno entity)
        {
           _context.Update(entity);
           await _context.SaveChangesAsync(); 
        }

        public async Task<List<Turno>> GetOcupacionesPorFechaYNumeroCancha(DateTime fechaInicio, DateTime fechaFin, int numeroCancha)
        {           
            var ocupaciones = await _context.Turnos.ToListAsync();

            
            var ocupacionesFiltradas = ocupaciones.Where(t =>
                t.NumeroCancha == numeroCancha &&
                ((t.FechaInicio >= fechaInicio && t.FechaInicio < fechaFin) ||
                 (t.FechaFinalizacion > fechaInicio && t.FechaFinalizacion <= fechaFin) ||
                 (t.FechaInicio <= fechaInicio && t.FechaFinalizacion >= fechaInicio && t.FechaFinalizacion <= fechaFin) ||
                 (t.FechaInicio >= fechaInicio && t.FechaInicio <= fechaFin && t.FechaFinalizacion >= fechaFin))).ToList();

            return ocupacionesFiltradas;
        }

        public async Task<bool> ExisteOcupacion(DateTime fechaInicio, DateTime fechaFin, int numeroCancha)
        {            
            var ocupaciones = await GetOcupacionesPorFechaYNumeroCancha(fechaInicio, fechaFin, numeroCancha);
            
            var ocupacionesFiltradas = ocupaciones.Where(t =>
                (t.FechaInicio >= fechaInicio && t.FechaInicio < fechaFin) ||
                (t.FechaFinalizacion > fechaInicio && t.FechaFinalizacion <= fechaFin) ||
                (t.FechaInicio <= fechaInicio && t.FechaFinalizacion >= fechaFin)
            );
           
            return ocupacionesFiltradas.Any();
        }       


       
    }

}
