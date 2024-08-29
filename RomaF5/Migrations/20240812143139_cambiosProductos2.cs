using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RomaF5.Migrations
{
    public partial class cambiosProductos2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Imagen",
                table: "Productos",
                newName: "RutaImagen");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RutaImagen",
                table: "Productos",
                newName: "Imagen");
        }
    }
}
