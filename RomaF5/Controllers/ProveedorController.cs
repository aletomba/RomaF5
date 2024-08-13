using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RomaF5.IRepository;
using RomaF5.Models;

namespace RomaF5.Controllers
{
    [Authorize] 
    public class ProveedorController : Controller
    {
        private readonly ProveedorRepository _repository;

        public ProveedorController(ProveedorRepository repository)
        {
            _repository = repository;    
        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var proveedores = await _repository.GetAllAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                proveedores = proveedores.Where(p => p.Nombre.Contains(searchString)).ToList();
            }

            return View(proveedores);
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(Proveedor proveedor)
        {
            await _repository.AddAsync(proveedor);
            return RedirectToAction("Index");
        }
       
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> CambiarPrecio(int id)
        {
            var proveedor = await _repository.GetbyId(id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarPrecio(int id, decimal precio)
        {
            if (id <= 0 || precio < 0)
            {
                return BadRequest();
            }

            var proveedor = await _repository.GetbyId(id);
            if (proveedor == null)
            {
                return NotFound();
            }

            proveedor.Productos = proveedor.Productos.Select(p =>
            {
                p.Precio = p.Precio * (1 + (precio / 100));
                p.CambiarPorcentaje(p);
                return p;
            }).ToList();

            await _repository.Update(proveedor);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "ADMIN")]
        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _repository.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var producto = await _repository.GetByIdAsync(id);
            if (producto != null)
            {
                await _repository.Delete(producto);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
