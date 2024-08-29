using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RomaF5.Migrations
{
    public partial class tipodepagoaventas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoPago",
                table: "Ventas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoPago",
                table: "Ventas");
        }
    }
}
