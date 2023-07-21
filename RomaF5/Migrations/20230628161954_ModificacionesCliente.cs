using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RomaF5.Migrations
{
    public partial class ModificacionesCliente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DNI",
                table: "Clientes");

            migrationBuilder.AddColumn<int>(
                name: "NumeroCel",
                table: "Clientes",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroCel",
                table: "Clientes");

            migrationBuilder.AddColumn<string>(
                name: "DNI",
                table: "Clientes",
                type: "TEXT",
                nullable: true);
        }
    }
}
