using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JarredsOrderHub.Migrations
{
    /// <inheritdoc />
    public partial class ActualizacionPedidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdRepartidor",
                table: "Pedidos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LatitudEntrega",
                table: "Pedidos",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LongitudEntrega",
                table: "Pedidos",
                type: "float",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Descripcion", "Nombre", "Permisos" },
                values: new object[,]
                {
                    { 2, "Acceso a pedidos e historiales", "Cocinero", "Administrar Pedidos, Ver Tareas, Ver tareas" },
                    { 3, "Acceso a ciertos pedidos asignados", "Repartidor", "Administrar usuarios, Ver tareas, Historial de Pedidos entregados" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "IdRepartidor",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "LatitudEntrega",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "LongitudEntrega",
                table: "Pedidos");
        }
    }
}
