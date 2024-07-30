using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList;
using RomaF5.IRepository;
using RomaF5.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

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
        public async Task <IActionResult> Create()
        {
            ViewBag.Clientes = await _clienteRepo.GetAllAsync(); 
            ViewBag.Productos = await _prodRepo.GetAllAsync();
            return View();
        }

        // POST: Ventas/Create       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venta venta)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    decimal total = 0;
                    foreach (var ventaProducto in venta.VentasProductos)
                    {
                        var prod = await _prodRepo.GetAllAsync();
                        var product = prod.FirstOrDefault(x => x.Id == ventaProducto.ProductoId);
                        if (product != null && ventaProducto.Cantidad > 0)
                        {
                            total += ventaProducto.Cantidad * product.Precio;
                            product.DescontarStock(ventaProducto.Cantidad);
                        }
                    }
                    venta.Total = total;

                    await _ventaRepository.AddAsync(venta);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                
                ViewBag.StockAlert = ex.Message;             
                
            }
          

            ViewBag.Clientes = await _clienteRepo.GetAllAsync();
            ViewBag.Productos = await _prodRepo.GetAllAsync();
            return View(venta);
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
            ViewData["ClienteId"] = new SelectList(await _clienteRepo.GetAllAsync(), "Id", "Id", venta.ClienteId);
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
            ViewData["ClienteId"] = new SelectList(await _clienteRepo.GetAllAsync(), "Id", "Id", venta.ClienteId);
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


    }
}
