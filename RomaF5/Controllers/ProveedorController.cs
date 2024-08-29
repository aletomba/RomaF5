using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RomaF5.IRepository;
using RomaF5.Models;

namespace RomaF5.Controllers
{
    [Authorize] 
    public class ProveedorController : Controller
    {
        private readonly ProveedorRepository _repository;
        private readonly ProductoRepo _productoRepo;    

        public ProveedorController(ProveedorRepository repository, ProductoRepo productoRepo)
        {
            _repository = repository;
            _productoRepo = productoRepo;   
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
        public async Task<IActionResult> CambiarPrecio(int id, decimal precioCompra, decimal precioVentaMin, decimal precioVentamay)
        {
            //var proveedor = await _repository.GetbyId(id);

            // Obtén los productos asociados a ese proveedor
            var productos = await _repository.GetProductosByProveedorId(id);

            // Itera sobre los productos y actualiza el precio de cada uno
            foreach (var producto in productos)
            {
                if(precioCompra > 0)
                {
                    producto.Precio = producto.Precio * (1 + precioCompra / 100);
                }
                if(precioVentaMin > 0)
                {
                    producto.PrecioVenta = producto.PrecioVenta * (1 + precioVentaMin / 100);
                }
                if(precioVentamay > 0)
                {
                    producto.PrecioMayorista = producto.PrecioMayorista * (1 + precioVentamay / 100);
                }               

            }

            // Guarda los cambios en la base de datos
            await _productoRepo.UpdateForProv(productos);
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

        [Authorize(Roles = "ADMIN")]
        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _repository.GetByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Proveedor proveedor)
        {
            if (id != proveedor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.Update(proveedor);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }
    }
}
