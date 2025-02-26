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
            migrationBuilder.CreateTable(
    name: "Tareas",
    columns: table => new
    {
        IdTarea = table.Column<int>(type: "int", nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
        NombreTarea = table.Column<string>(type: "nvarchar(max)", nullable: false),
        Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
        FechaEntrega = table.Column<DateTime>(type: "datetime2", nullable: true),
        IdEmpleado = table.Column<int>(type: "int", nullable: true)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_Tareas", x => x.IdTarea);
        table.ForeignKey(
            name: "FK_Tareas_Empleados_IdEmpleado",
            column: x => x.IdEmpleado,
            principalTable: "Empleados",
            principalColumn: "IdEmpleado",
            onDelete: ReferentialAction.Cascade);
    });

            migrationBuilder.CreateIndex(
    name: "IX_Tareas_IdEmpleado",
    table: "Tareas",
    column: "IdEmpleado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
            name: "Tareas");

        }
    }
}
