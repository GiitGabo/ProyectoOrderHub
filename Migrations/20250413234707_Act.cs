using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JarredsOrderHub.Migrations
{
    /// <inheritdoc />
    public partial class Act : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClienteIdCliente",
                table: "Pedidos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_ClienteIdCliente",
                table: "Pedidos",
                column: "ClienteIdCliente");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Clientes_ClienteIdCliente",
                table: "Pedidos",
                column: "ClienteIdCliente",
                principalTable: "Clientes",
                principalColumn: "IdCliente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Clientes_ClienteIdCliente",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_ClienteIdCliente",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "ClienteIdCliente",
                table: "Pedidos");
        }
    }
}
