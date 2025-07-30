using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JarredsOrderHub.Migrations
{
    public partial class ActualizacionCLI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Agrega una nueva columna de tipo int
            migrationBuilder.AddColumn<int>(
                name: "NumeroContacto",
                table: "Clientes",
                type: "int",
                nullable: true,
                defaultValue: 0);
        }
    }
}
