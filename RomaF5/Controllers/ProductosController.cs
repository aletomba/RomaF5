using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagedList;
using RomaF5.IRepository;
using RomaF5.Models;

namespace RomaF5.Controllers
{
    [Authorize]
	public class ProductosController : Controller
    {
		private readonly IRepository<Producto> _productoRepository;
       


        public ProductosController(IRepository<Producto> productoRepository)
        {
			_productoRepository = productoRepository;           
        }

        // GET: Productos
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10; // Define el número de elementos por página
            int pageNumber = page ?? 1;

            var prodcuto = await _productoRepository.GetAllAsync();
            var productosPaginados = prodcuto.ToPagedList(pageNumber, pageSize);    
            
            return View(productosPaginados);
        }
        [Authorize(Roles ="ADMIN")]
        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _productoRepository.GetByIdAsync(id); 
            
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }
        [Authorize(Roles = "ADMIN")]
        // GET: Productos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Precio,Stock")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                await _productoRepository.AddAsync(producto);               
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }
        [Authorize(Roles = "ADMIN")]
        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _productoRepository.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Precio,Stock")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                  
					await _productoRepository.Update(producto);
                   
                }
                catch (Exception)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }
        [Authorize(Roles = "ADMIN")]
        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var producto = await _productoRepository.GetByIdAsync(id);
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

            var producto = await _productoRepository.GetByIdAsync(id);
            if (producto != null)
            {
				await _productoRepository.Delete(producto);
            }            
            
            return RedirectToAction(nameof(Index));      
        }

     
    }
}
