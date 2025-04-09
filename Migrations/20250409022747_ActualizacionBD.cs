using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JarredsOrderHub.Migrations
{
    /// <inheritdoc />
    public partial class ActualizacionBD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reportes_Clientes_IdCliente",
                table: "Reportes");

            migrationBuilder.AlterColumn<int>(
                name: "IdCliente",
                table: "Reportes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reportes_Clientes_IdCliente",
                table: "Reportes",
                column: "IdCliente",
                principalTable: "Clientes",
                principalColumn: "IdCliente",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reportes_Clientes_IdCliente",
                table: "Reportes");

            migrationBuilder.AlterColumn<int>(
                name: "IdCliente",
                table: "Reportes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Reportes_Clientes_IdCliente",
                table: "Reportes",
                column: "IdCliente",
                principalTable: "Clientes",
                principalColumn: "IdCliente");
        }
    }
}
