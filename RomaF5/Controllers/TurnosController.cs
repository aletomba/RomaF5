using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList;
using RomaF5.IRepository;
using RomaF5.Models;

namespace RomaF5.Controllers
{
    [Authorize]
    public class TurnosController : Controller
    {
        private readonly TurnoRepository _turnoRepo;
        private readonly IRepository<Cliente> _clienteRepo;
        

        public TurnosController(TurnoRepository turnoRepo, IRepository<Cliente> productoRepository)
        {
            _turnoRepo = turnoRepo;
            _clienteRepo = productoRepository;
          
        }

        // GET: Turnos
        public async Task< IActionResult> Index(int? page)
        {
            int pageSize = 10; // Define el número de elementos por página
            int pageNumber = page ?? 1;


            var turnosPaginados = await _turnoRepo.GetAllAsync();
            var resultadoPaginado = turnosPaginados.ToPagedList(pageNumber, pageSize);

            return View(resultadoPaginado);          
        }

        // GET: Turnos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _turnoRepo.GetByIdAsync(id); 
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        // GET: Turnos/Create
        public IActionResult Create()
        {
         
            ViewData["ClienteId"] = new SelectList(_clienteRepo.GetAllAsync().Result, "Id", "Nombre");
            return View();
        }

        // POST: Turnos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,FechaInicio,FechaFinalizacion,NumeroCancha,Pagado,ClienteId")] Turno turno)
        {            
            if (ModelState.IsValid)
            {              
                if (!await _turnoRepo.ExisteOcupacion(turno.FechaInicio,turno.FechaFinalizacion, turno.NumeroCancha))
                {
                    await _turnoRepo.AddAsync(turno);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Mensaje"] = "El turno no está disponible. Por favor, elija otro horario.";
                    ViewData["ClienteId"] = new SelectList(_clienteRepo.GetAllAsync().Result, "Id", "Nombre", turno.ClienteId);
                    return View(turno);
                }
                
            }
            ViewData["ClienteId"] = new SelectList(_clienteRepo.GetAllAsync().Result, "Id", "Nombre", turno.ClienteId);
            return View(turno);
        }

        // GET: Turnos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _turnoRepo.GetByIdAsync(id);
            if (turno == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_clienteRepo.GetAllAsync().Result, "Id", "Nombre");
            return View(turno);
        }

        // POST: Turnos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Turno turno)
        {
            if (id != turno.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _turnoRepo.Update(turno);
                }
                catch (DbUpdateConcurrencyException)
                {                  
                   return NotFound();   // corregir el error que se lanza             
                    
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_clienteRepo.GetAllAsync().Result, "Id", "Nombre", turno.ClienteId);
            return View(turno);
        }

        // GET: Turnos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _turnoRepo.GetByIdAsync(id);
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        // POST: Turnos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {            
            var turno = await _turnoRepo.GetByIdAsync(id);
            if (turno != null)
            {
                await _turnoRepo.Delete(turno);
            }          
            
            return RedirectToAction(nameof(Index));
        }
       
    }
}
