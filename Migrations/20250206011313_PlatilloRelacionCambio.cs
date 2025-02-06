using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JarredsOrderHub.Migrations
{
    /// <inheritdoc />
    public partial class PlatilloRelacionCambio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Platillos_Categorias_IdCategoria",
                table: "Platillos");

            migrationBuilder.AddForeignKey(
                name: "FK_Platillos_Categorias_IdCategoria",
                table: "Platillos",
                column: "IdCategoria",
                principalTable: "Categorias",
                principalColumn: "IdCategoria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Platillos_Categorias_IdCategoria",
                table: "Platillos");

            migrationBuilder.AddForeignKey(
                name: "FK_Platillos_Categorias_IdCategoria",
                table: "Platillos",
                column: "IdCategoria",
                principalTable: "Categorias",
                principalColumn: "IdCategoria",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
