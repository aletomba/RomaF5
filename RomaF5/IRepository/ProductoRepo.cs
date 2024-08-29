using Microsoft.EntityFrameworkCore;
using RomaF5.Data;
using RomaF5.Models;

namespace RomaF5.IRepository
{
    public class ProductoRepo : Repository<Producto>
    {
        private readonly ApplicationDbContext _context;
        public ProductoRepo(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }
        public async Task<List<Producto>> GetProducts()
        {
            var prod = await _context.Productos.Include(x => x.ProductoProveedores).
                                                ThenInclude(x => x.Proveedor).
                                                OrderBy(x => x.Nombre).
                                                ToListAsync();
                                                
            return prod;
        }
        public IEnumerable<Producto> Buscar(string searchTerm)
        {
            return _context.Productos
                .Where(p => p.Nombre.ToLower().Contains(searchTerm.ToLower()))
                .ToList();
        }

        public async Task <Producto> GetById(int? id)
        {
            return await _context.Productos.Include(x => x.ProductoProveedores).ThenInclude(p => p.Proveedor).FirstOrDefaultAsync(x=>x.Id ==id);
        }

        public async Task Save()
        {
                                    
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProd(Producto producto, List<int> proveedoresSeleccionados)
        {
            // Actualizar el producto
            var productoExistente = await _context.Productos
                                    .Include(p => p.ProductoProveedores)
                                    .ThenInclude(pp => pp.Proveedor)
                                    .FirstOrDefaultAsync(p => p.Id == producto.Id);

            productoExistente.Nombre=producto.Nombre;
            productoExistente.Precio=producto.Precio;   
            productoExistente.PrecioVenta=producto.PrecioVenta;
            productoExistente.PrecioMayorista=producto.PrecioMayorista; 
            productoExistente.Stock=producto.Stock;

            // Actualizar la relación MM con Proveedor
            foreach (var proveedorId in proveedoresSeleccionados)
            {
                var proveedor = await _context.Proveedor.FindAsync(proveedorId);
                if (proveedor != null)
                {
                    var productoProveedor = productoExistente.ProductoProveedores
                        .FirstOrDefault(pp => pp.ProveedorId == proveedorId);

                    if (productoProveedor == null)
                    {
                        productoExistente.ProductoProveedores.Add(new ProductoProveedor
                        {
                            Producto = productoExistente,
                            Proveedor = proveedor
                        });
                    }
                }
            }
            
            // Eliminar proveedores que no están seleccionados
            foreach (var productoProveedor in productoExistente.ProductoProveedores.ToList())
            {
                if (!proveedoresSeleccionados.Contains(productoProveedor.Proveedor.Id))
                {
                    productoExistente.ProductoProveedores.Remove(productoProveedor);
                }
            }

            // Guardar los cambios
            await _context.SaveChangesAsync();
        }

        public async Task UpdateForProv(List<Producto> productos)
        {
            try
            {
                foreach (var producto in productos)
                {
                    _context.Entry(producto).State = EntityState.Modified;
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Manejar la excepción
            }
        }
    }
}
