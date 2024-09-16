using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList;
using RomaF5.IRepository;
using RomaF5.Models;
using RomaF5.Models.productosViewModels;
using RomaF5.Service;
using System.Drawing.Printing;

namespace RomaF5.Controllers
{
    [Authorize]
	public class ProductosController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ProveedorRepository _proveedorRepository;
        private readonly ProductoRepo _productorRepo;
        private readonly IPaginationService _pageService;   


        public ProductosController(ProveedorRepository proveedorRepository,
            ProductoRepo productorRepo, IWebHostEnvironment webHostEnvironment, IPaginationService pageService)
        {			      
            _proveedorRepository = proveedorRepository;
            _productorRepo = productorRepo;
            _webHostEnvironment = webHostEnvironment;
            _pageService = pageService;
        }

        // GET: Productos
        [HttpGet]
        public async Task<IActionResult> IndexPdf()
        {
            var producto =  await _productorRepo.GetProducts();
            return View(producto);
        }
    
        public async Task<IActionResult> Index(int id,int? page, int pageSize = 10, string searchQuery = null)
        {
            if(id != 0)
            {
                ViewBag.Id = id;
                ViewBag.SearchQuery = searchQuery;
                if (!string.IsNullOrEmpty(searchQuery))
                {

                    var proveedor = await _proveedorRepository.GetProductosByProveedorId(id);
                    if (proveedor == null)
                    {
                        return NotFound();
                    }
                    var productosXProvPaginados = proveedor.ToPagedList(page ?? 1, pageSize);

                    return View(productosXProvPaginados);
                }
                else
                {
                    var proveedor = await _proveedorRepository.GetProductosByProveedorId(id);
                    if (proveedor == null)
                    {
                        return NotFound();
                    }

                    var productosXProvPaginados = proveedor.ToPagedList(page ?? 1, pageSize);

                    return View(productosXProvPaginados);
                }                 
             
            }//falta que dentro de un proveedor busque un nombre
            else
            {
                ViewBag.SearchQuery = searchQuery;
                if (!string.IsNullOrEmpty(searchQuery))
                {

                    var productos = _productorRepo.Buscar(searchQuery);
                    return View(productos.ToPagedList(page ?? 1, pageSize));
                }
                else
                {
                    var productos = await _productorRepo.GetProducts();
                    return View(productos.ToPagedList(page ?? 1, pageSize));
                }
            }
           
        }
        //public ActionResult IndexSearch(string searchTerm, int? page)
        //{
        //    if (string.IsNullOrEmpty(searchTerm))
        //    {
        //        return RedirectToAction("Index"); // o return View("Index"); si prefieres mostrar la vista original
        //    }
        //    int pageSize = 10; // Define el número de elementos por página
        //    int pageNumber = page ?? 1;
        //    ViewBag.ActionName = "IndexSearch";
        //    ViewBag.SearchQuery = searchTerm;
        //    // Método que muestra los resultados de la búsqueda
        //    var productos = _productorRepo.Buscar(searchTerm);
        //    var productosPaginados = productos.ToPagedList(pageNumber, pageSize);
        
        //    return View("Index", productosPaginados);
        //}

        //[HttpGet]
        //public ActionResult Buscar(string searchTerm, int? page)
        //{
        //    int pageSize = 10; // Define el número de elementos por página
        //    int pageNumber = page ?? 1;
        //    var productos = _productorRepo.Buscar(searchTerm);
        //    var productosPaginados = productos.ToPagedList(pageNumber, pageSize);
        //    return Json(productosPaginados);
        //}

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
        public async Task<IActionResult> Create([Bind("Id,Nombre,Precio,Stock,Proveedores")] Producto producto, int[] proveedoresSeleccionados)
        {
            ModelState.Remove("RutaImagen");//lo saco del modelstate por que si no da error
            if (ModelState.IsValid)
            {
                foreach (var proveedorId in proveedoresSeleccionados)
                {
                    var proveedor = await _proveedorRepository.GetByIdAsync(proveedorId);
                    if (proveedor != null)
                    {
                        var productoProveedor = new ProductoProveedor
                        {
                            Producto = producto,
                            Proveedor = proveedor
                        };
                        producto.ProductoProveedores.Add(productoProveedor);            
                       
                    }
                }
                producto.CambiarPorcentaje(producto);

                //if(producto.RutaImagen != null)
                //{
                //    var rutaImagen = Path.Combine(_webHostEnvironment.WebRootPath, "img", producto.Imagen.FileName);
                //    using (var stream = new FileStream(rutaImagen, FileMode.Create))
                //    {
                //        producto.Imagen.CopyTo(stream);
                //    }
                //    producto.RutaImagen = $"~/img/{producto.Imagen.FileName}";

                //}               

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

            var producto = await _productorRepo.GetById(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["Proveedores"] = new SelectList(await _proveedorRepository.GetAllAsync(), "Id", "Nombre");
            
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Producto producto, List<int> proveedoresSeleccionados)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _productorRepo.UpdateProd(producto,proveedoresSeleccionados);
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
