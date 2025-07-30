using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JarredsOrderHub.Migrations
{
    /// <inheritdoc />
    public partial class ActuCli : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Agrega una nueva columna de tipo int
            migrationBuilder.AddColumn<int>(
                name: "NumeroContacto",
                table: "Clientes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Elimina la columna agregada
            migrationBuilder.DropColumn(
                name: "NumeroContactoInt",
                table: "Clientes");
        }
    }
}
