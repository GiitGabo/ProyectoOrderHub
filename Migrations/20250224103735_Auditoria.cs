using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JarredsOrderHub.Migrations
{
    /// <inheritdoc />
    public partial class Auditoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HorarioIdHorario",
                table: "Empleados",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RolId",
                table: "Empleados",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoEntidad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntidadId = table.Column<int>(type: "int", nullable: false),
                    Accion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaAccion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DetallesCambios = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Horarios",
                columns: table => new
                {
                    IdHorario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoraInicio = table.Column<TimeSpan>(type: "time", nullable: false),
                    HoraFin = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horarios", x => x.IdHorario);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_HorarioIdHorario",
                table: "Empleados",
                column: "HorarioIdHorario");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_RolId",
                table: "Empleados",
                column: "RolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Horarios_HorarioIdHorario",
                table: "Empleados",
                column: "HorarioIdHorario",
                principalTable: "Horarios",
                principalColumn: "IdHorario",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Roles_RolId",
                table: "Empleados",
                column: "RolId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Horarios_HorarioIdHorario",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Roles_RolId",
                table: "Empleados");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "Horarios");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_HorarioIdHorario",
                table: "Empleados");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_RolId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "HorarioIdHorario",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "RolId",
                table: "Empleados");
        }
    }
}
