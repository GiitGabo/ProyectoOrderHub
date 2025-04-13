using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JarredsOrderHub.Migrations
{
    public partial class Migracion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tabla Cupones
            migrationBuilder.CreateTable(
                name: "Cupones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                              .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EsPorcentual = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cupones", x => x.Id);
                });

            // Tabla Pedidos
            migrationBuilder.CreateTable(
                 name: "Pedidos",
                 columns: table => new
                 {
                     Id = table.Column<int>(type: "int", nullable: false)
                               .Annotation("SqlServer:Identity", "1, 1"),
                     UsuarioId = table.Column<int>(type: "int", nullable: false),
                     FechaPedido = table.Column<DateTime>(type: "datetime2", nullable: false),
                     EstadoPedido = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     MetodoPago = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                     Comentarios = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     CuponId = table.Column<int>(type: "int", nullable: true)
                 },
                 constraints: table =>
                 {
                     table.PrimaryKey("PK_Pedidos", x => x.Id);
                     table.ForeignKey(
                         name: "FK_Pedidos_Cupones",
                         column: x => x.CuponId,
                         principalTable: "Cupones",
                         principalColumn: "Id",
                         onDelete: ReferentialAction.Restrict);
                 });

            // Tabla CuponClientes
            migrationBuilder.CreateTable(
                name: "CuponClientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                              .Annotation("SqlServer:Identity", "1, 1"),
                    CuponId = table.Column<int>(type: "int", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    FechaUso = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuponClientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CuponClientes_Cupones",
                        column: x => x.CuponId,
                        principalTable: "Cupones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CuponClientes_Clientes",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                });

            // Tabla DetallePedidos
            migrationBuilder.CreateTable(
                name: "DetallePedidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                              .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallePedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallePedidos_Pedidos",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Tabla Pagos
            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                              .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagos_Pedidos",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Creación de índices con los nombres correctos
            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_CuponId",
                table: "Pedidos",
                column: "CuponId");

            migrationBuilder.CreateIndex(
                name: "IX_CuponClientes_CuponId",
                table: "CuponClientes",
                column: "CuponId");

            migrationBuilder.CreateIndex(
                name: "IX_CuponClientes_ClienteId",
                table: "CuponClientes",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallePedidos_PedidoId",
                table: "DetallePedidos",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_PedidoId",
                table: "Pagos",
                column: "PedidoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Pagos");
            migrationBuilder.DropTable(name: "DetallePedidos");
            migrationBuilder.DropTable(name: "CuponClientes");
            migrationBuilder.DropTable(name: "Pedidos");
            migrationBuilder.DropTable(name: "Cupones");
        }
    }
}
