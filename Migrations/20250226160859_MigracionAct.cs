using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JarredsOrderHub.Migrations
{
    /// <inheritdoc />
    public partial class MigracionAct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Horarios_HorarioIdHorario",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Roles_RolId",
                table: "Empleados");

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

            migrationBuilder.AlterColumn<string>(
                name: "Contrasenia",
                table: "Empleados",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

            migrationBuilder.InsertData(
                table: "Horarios",
                columns: new[] { "IdHorario", "HoraFin", "HoraInicio" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 8, 0, 0, 0) },
                    { 2, new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0) },
                    { 3, new TimeSpan(0, 19, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Descripcion", "Nombre", "Permisos" },
                values: new object[] { 1, "Acceso total a la pagina.", "Administrador", "Administrar usuarios, Ver empleados, Ver tareas, Administrar tareas" });

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_IdHorario",
                table: "Empleados",
                column: "IdHorario");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_IdRol",
                table: "Empleados",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_IdEmpleado",
                table: "Tareas",
                column: "IdEmpleado");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Horarios_IdHorario",
                table: "Empleados",
                column: "IdHorario",
                principalTable: "Horarios",
                principalColumn: "IdHorario");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Roles_IdRol",
                table: "Empleados",
                column: "IdRol",
                principalTable: "Roles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Horarios_IdHorario",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Roles_IdRol",
                table: "Empleados");

            migrationBuilder.DropTable(
                name: "Tareas");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_IdHorario",
                table: "Empleados");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_IdRol",
                table: "Empleados");

            migrationBuilder.DeleteData(
                table: "Horarios",
                keyColumn: "IdHorario",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Horarios",
                keyColumn: "IdHorario",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Horarios",
                keyColumn: "IdHorario",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Contrasenia",
                table: "Empleados",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}
