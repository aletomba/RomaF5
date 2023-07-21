
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RomaF5.Models;
using System.Reflection.Emit;

namespace RomaF5.Data
{
	public class ApplicationDbContext : IdentityDbContext
    { 
    
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		// Agrega los DbSet para tus entidades
		public DbSet<Venta> Ventas { get; set; }
		public DbSet<Cancha> Canchas { get; set; }
		public DbSet<Cliente> Clientes { get; set; }
		public DbSet<Producto> Productos { get; set; }
		public DbSet<Turno> Turnos { get; set; }
		public DbSet<VentaProducto> VentaProductos { get; set; }
        // ...
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite();

        // Otros métodos y configuraciones
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
		{
			base.OnModelCreating(modelBuilder);
            // Configuración de las relaciones

            modelBuilder.Entity<Venta>()
				.HasMany(v => v.VentasProductos)
				.WithOne(vp => vp.Venta)
				.HasForeignKey(vp => vp.VentaId);

			modelBuilder.Entity<Producto>()
				.HasMany(p => p.VentasProductos)
				.WithOne(vp => vp.Producto)
				.HasForeignKey(vp => vp.ProductoId);

			modelBuilder.Entity<VentaProducto>()
				.HasKey(vp => new { vp.VentaId, vp.ProductoId });

         //   modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
         //   {
         //       entity.HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId });
         //   });

         //   modelBuilder.Entity<IdentityUserRole<string>>().HasKey(ur => new { ur.UserId, ur.RoleId });

         //   modelBuilder.Entity<IdentityUserRole<string>>()
									//.HasKey(r => new { r.UserId, r.RoleId });
            // ...
        }
	}

}
