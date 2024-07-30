using Microsoft.AspNetCore.Mvc;
using RomaF5.IRepository;
using RomaF5.Models;
using RomaF5.Models.productosViewModels;
using Rotativa.AspNetCore;

namespace RomaF5.Controllers
{
    public class PdfController : Controller
    {
        private readonly ProductoRepo _productoRepo;
        public PdfController(ProductoRepo productoRepo)
        {
            _productoRepo = productoRepo;     
        }


        public async Task<IActionResult> GenerarPdf(bool esMayorista)
        {
            var viewModel = new ProductoViewModel { EsMayorista = esMayorista };           
            var productos = await ObtenerProductos(); // Aquí debes obtener tus productos de alguna manera
            viewModel.Productos = productos;

            return new ViewAsPdf("/Views/Productos/IndexPdf.cshtml", viewModel)
            {
                FileName = "Lista_Productos.pdf",
                CustomSwitches = "--print-media-type --no-background", // Añadir esta línea
               
            };
        }


        private async Task<List<Producto>> ObtenerProductos()
        {
            var productos = await _productoRepo.GetAllAsync();
            return productos;
        }
    }
}
