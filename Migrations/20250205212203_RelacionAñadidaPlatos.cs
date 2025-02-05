using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JarredsOrderHub.Migrations
{
    /// <inheritdoc />
    public partial class RelacionAñadidaPlatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Platillos_IdCategoria",
                table: "Platillos",
                column: "IdCategoria");

            migrationBuilder.AddForeignKey(
                name: "FK_Platillos_Categorias_IdCategoria",
                table: "Platillos",
                column: "IdCategoria",
                principalTable: "Categorias",
                principalColumn: "IdCategoria",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Platillos_Categorias_IdCategoria",
                table: "Platillos");

            migrationBuilder.DropIndex(
                name: "IX_Platillos_IdCategoria",
                table: "Platillos");
        }
    }
}
