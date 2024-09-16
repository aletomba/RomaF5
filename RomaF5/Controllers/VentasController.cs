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
    public class VentasController : Controller
    {
        private readonly VentaRepository _ventaRepository;
        private readonly IRepository<Producto> _prodRepo;
        private readonly IRepository<Cliente> _clienteRepo;
    

        public VentasController(VentaRepository ventaRepository, IRepository<Producto> prodRepo, 
            IRepository<Cliente> clienteRepo)
        {
            _ventaRepository = ventaRepository;
            _prodRepo = prodRepo;
            _clienteRepo = clienteRepo;          
        }

        // GET: Ventas
        public async Task<IActionResult> Index(int? page, DateTime? fechaFiltro)
        {
            int pageSize = 5; // Define el número de elementos por página
            int pageNumber = page ?? 1;

            if (fechaFiltro.HasValue)
            {
                // Filtrar las ventas por fecha
                var ventasFecha = await _ventaRepository.GetByDate(fechaFiltro.Value);
                var ventasFechaPag = ventasFecha.ToPagedList(pageNumber, pageSize);
                return View(ventasFechaPag);
            }
        

            var ventas = await _ventaRepository.GetAllAsync();
            var ventasPaginado = ventas.ToPagedList(pageNumber, pageSize);
            return View(ventasPaginado);
        }

        [Authorize(Roles = "ADMIN")]
        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _ventaRepository.GetByIdAsync(id);  
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // GET: Ventas/Create
        public async Task <IActionResult> Create(int? page, int pageSize = 10)
        {
            var tipoDePagos = Enum.GetValues(typeof(TipoPago)).Cast<TipoPago>().ToList();
            var venta = new Venta { TipoPago = TipoPago.Efectivo };
            ViewBag.Clientes = await _clienteRepo.GetAllAsync(); 
            var productos = await _prodRepo.GetAllAsync();
            ViewBag.Productos = productos;
            ViewBag.TipoDePago = tipoDePagos;   
            return View(venta);
        }

        // POST: Ventas/Create       
        [HttpPost]       
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venta venta, Cuota cuota)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (venta.NumeroCuotas > 0)
                    {
                        cuota.Pagada = true;
                        venta.Cuotas.Add(cuota);
                        
                       
                        decimal? total = 0;
                        var cliente = await _clienteRepo.GetByIdAsync(venta.ClienteId);
                        foreach (var ventaProducto in venta.VentasProductos)
                        {
                            var prod = await _prodRepo.GetByIdAsync(ventaProducto.ProductoId);
                            if (prod != null && ventaProducto.Cantidad > 0)
                            {
                                decimal? precio = cliente.Nombre == "Minorista" ? prod.PrecioVenta : prod.PrecioMayorista;
                                total += ventaProducto.Cantidad * precio;                               
                            }
                        }
                        venta.Total = total;
                        venta.MontoPagado = cuota.Monto;
                    }
                    else
                    {
                        decimal? total = 0;
                        var cliente = await _clienteRepo.GetByIdAsync(venta.ClienteId);
                        foreach (var ventaProducto in venta.VentasProductos)
                        {
                            var prod = await _prodRepo.GetByIdAsync(ventaProducto.ProductoId);
                            if (prod != null && ventaProducto.Cantidad > 0)
                            {
                                decimal? precio = cliente.Nombre == "Minorista" ? prod.PrecioVenta : prod.PrecioMayorista;
                                total += ventaProducto.Cantidad * precio;
                                prod.DescontarStock(ventaProducto.Cantidad);
                            }
                        }
                        venta.Total = total;
                    }                 

                    await _ventaRepository.AddAsync(venta);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.StockAlert = ex.Message;
            }

            var tipoDePagos = Enum.GetValues(typeof(TipoPago)).Cast<TipoPago>().ToList();
            var pago = new Venta { TipoPago = TipoPago.Efectivo };
            ViewBag.Clientes = await _clienteRepo.GetAllAsync();
            ViewBag.Productos = await _prodRepo.GetAllAsync();
            ViewBag.TipoDePago = tipoDePagos;
            return View(pago);
        }


        // GET: Ventas/Edit/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _ventaRepository.GetByIdAsync(id);
            if (venta == null)
            {
                return NotFound();
            }
            ViewData["ClienteNombre"] = new SelectList(await _clienteRepo.GetAllAsync(), "Id", "Nombre", venta.Cliente.Nombre);
            ViewData["ProductoNombre"] = new SelectList(await _prodRepo.GetAllAsync(), "Id", "Nombre", venta.VentasProductos.FirstOrDefault().Producto.Nombre);
            return View(venta);
        }

        // POST: Ventas/Edit/5        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,ClienteId")] Venta venta)
        {
            if (id != venta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _ventaRepository.Update(venta);
                }
                catch (DbUpdateConcurrencyException)
                {
                   // manejar excepciones                 
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteNombre"] = new SelectList(await _clienteRepo.GetAllAsync(), "Id", "Nombre", venta.Cliente.Nombre);
            return View(venta);
        }

        // GET: Ventas/Delete/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _ventaRepository.GetByIdAsync(id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {         
            var venta = await _ventaRepository.GetByIdAsync(id);
            if (venta != null)
            {
               await _ventaRepository.Delete(venta);
            }            
           
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> FiltrarPorFecha(DateTime fecha)
        {
            var ventas = await _ventaRepository.GetAllAsync();
            ventas = ventas.Where(v => v.Fecha.Date == fecha.Date).ToList();

            var totalDia = ventas.Sum(v => v.Total ?? 0);

            ViewBag.TotalDia = totalDia;
            return View("Index", ventas);
        }

        [HttpGet]
        public IActionResult _Cuotas()
        {         
            return PartialView("_Cuotas");
        }

        [HttpGet]
        public async Task<IActionResult> PagarCuota(int id)
        {
            // Lógica para pagar la cuota
            var venta = await _ventaRepository.GetByIdAsync(id);            
            venta.CuotasPagas = venta.Cuotas.Count();

            var cliente = await _clienteRepo.GetAllAsync();
            var clienteVenta = cliente.Where(x => x.Id == venta.ClienteId).ToList();

            var producto = await _prodRepo.GetAllAsync();         
            ViewData["ClienteNombre"] = new SelectList(clienteVenta, "Id", "Nombre");
            ViewBag.ProductosVenta = venta.VentasProductos.Select(vp => vp.Producto.Nombre).ToList();
            return View(venta);
        }
        [HttpPost]
        public async Task<IActionResult> PagarCuota(int id, decimal monto)
        {
            // Lógica para pagar la cuota
            try
            {                
                var resultado = await _ventaRepository.PagarCuota(id, monto);
                ViewBag.Mensaje = resultado switch
                {
                    // Cuotas pendientes
                    1 => "Le quedan cuotas por pagar",
                    // Cuotas pagadas
                    3 => "Has terminado de pagar las cuotas",
               
                    
                };
            }
            catch (Exception ex) { }
           
            return RedirectToAction("Index");
        }

    }
}
