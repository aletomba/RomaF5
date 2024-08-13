using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagedList;
using RomaF5.IRepository;
using RomaF5.Models;

namespace RomaF5.Controllers
{
    [Authorize]
	public class ProductosController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ProveedorRepository _proveedorRepository;
        private readonly ProductoRepo _productorRepo;


        public ProductosController(ProveedorRepository proveedorRepository,
            ProductoRepo productorRepo, IWebHostEnvironment webHostEnvironment)
        {			      
            _proveedorRepository = proveedorRepository;
            _productorRepo = productorRepo;
            _webHostEnvironment = webHostEnvironment;   
        }

        // GET: Productos
        [HttpGet]
        public async Task<IActionResult> IndexPdf()
        {
            var producto =  await _productorRepo.GetProducts();
            return View(producto);
        }
        [HttpGet]
        public async Task<IActionResult> Index(int? page, int id)
        {
            int pageSize = 10; // Define el número de elementos por página
            int pageNumber = page ?? 1;
            
            if (id == 0)
            {
                var prodcuto = await _productorRepo.GetProducts();
                var productosPaginados = prodcuto.ToPagedList(pageNumber, pageSize);

                return View(productosPaginados);
            }

            var proveedor = await _proveedorRepository.GetbyId(id);
            if(proveedor == null)
            {
                return NotFound();
            }

            var productosXProvPaginados = proveedor.Productos.ToPagedList(pageNumber, pageSize);

            return View(productosXProvPaginados);
         
        }
       
        [Authorize(Roles ="ADMIN")]
        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _productorRepo.GetByIdAsync(id); 
            
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }
        [Authorize(Roles = "ADMIN")]
        // GET: Productos/Create
        public async Task <IActionResult> Create()
        {
            var proveedores = await _proveedorRepository.GetAllAsync();
            ViewBag.Proveedores = proveedores.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Nombre }).ToList();
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Precio,Stock,Proveedores,Imagen")] Producto producto, int[] proveedoresSeleccionados)
        {
            ModelState.Remove("RutaImagen");//lo saco del modelstate por que si no da error
            if (ModelState.IsValid)
            {
                foreach (var proveedorId in proveedoresSeleccionados)
                {
                    var proveedor = await _proveedorRepository.GetByIdAsync(proveedorId);
                    if (proveedor != null)
                    {
                        producto.Proveedores.Add(proveedor);
                    }
                }
                producto.CambiarPorcentaje(producto);

                var rutaImagen = Path.Combine(_webHostEnvironment.WebRootPath, "img", producto.Imagen.FileName);
                using (var stream = new FileStream(rutaImagen, FileMode.Create))
                {
                    producto.Imagen.CopyTo(stream);
                }
                producto.RutaImagen = $"~/img/{producto.Imagen.FileName}";

                await _productorRepo.AddAsync(producto);
                return RedirectToAction(nameof(Index));
            }
            var proveedores = await _proveedorRepository.GetAllAsync();
            ViewBag.Proveedores = new SelectList(proveedores, "Id", "Nombre");
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

            var producto = await _productorRepo.GetByIdAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Precio,PrecioVenta,PrecioMayorista,Stock")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                  
					await _productorRepo.Update(producto);
                   
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

            var producto = await _productorRepo.GetByIdAsync(id);
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

            var producto = await _productorRepo.GetByIdAsync(id);
            if (producto != null)
            {
				await _productorRepo.Delete(producto);
            }            
            
            return RedirectToAction(nameof(Index));      
        }

        public IActionResult DescargarPdfV()
        {
            return RedirectToAction("GenerarPdf", "Pdf");
        }
        public IActionResult DescargarPdfM()
        {
            return RedirectToAction("GenerarPdf", "Pdf",new {esMayorista = true});
        }


    }
}
