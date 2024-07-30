using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RomaF5.Migrations
{
    public partial class Preciomayorista : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PrecioMayorista",
                table: "Productos",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrecioMayorista",
                table: "Productos");
        }
    }
}
