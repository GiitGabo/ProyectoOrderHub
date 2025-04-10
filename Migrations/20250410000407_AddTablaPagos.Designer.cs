﻿// <auto-generated />
using System;
using JarredsOrderHub.DbaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JarredsOrderHub.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250410000407_AddTablaPagos")]
    partial class AddTablaPagos
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JarredsOrderHub.Models.AuditLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Accion")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("DetallesCambios")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("EntidadId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaAccion")
                        .HasColumnType("datetime2");

                    b.Property<string>("TipoEntidad")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Usuario")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("AuditLogs");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.Categoria", b =>
                {
                    b.Property<int>("IdCategoria")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCategoria"));

                    b.Property<bool>("Activa")
                        .HasColumnType("bit");

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdCategoria");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.Cliente", b =>
                {
                    b.Property<int>("IdCliente")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCliente"));

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Contrasenia")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("estado")
                        .HasColumnType("bit");

                    b.HasKey("IdCliente");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.Cupon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Codigo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Descuento")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("EsPorcentual")
                        .HasColumnType("bit");

                    b.Property<DateTime>("FechaExpiracion")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Cupones");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.CuponCliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClienteId")
                        .HasColumnType("int");

                    b.Property<int>("CuponId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaUso")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CuponId");

                    b.ToTable("CuponClientes");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.DetallePedido", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int>("PedidoId")
                        .HasColumnType("int");

                    b.Property<decimal>("PrecioUnitario")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductoId")
                        .HasColumnType("int");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("PedidoId");

                    b.HasIndex("ProductoId");

                    b.ToTable("DetallePedidos");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.Empleado", b =>
                {
                    b.Property<int>("IdEmpleado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdEmpleado"));

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Contrasenia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IdHorario")
                        .HasColumnType("int");

                    b.Property<int?>("IdRol")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Salario")
                        .HasColumnType("int");

                    b.Property<bool>("estado")
                        .HasColumnType("bit");

                    b.HasKey("IdEmpleado");

                    b.HasIndex("IdHorario");

                    b.HasIndex("IdRol");

                    b.ToTable("Empleados");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.Horario", b =>
                {
                    b.Property<int>("IdHorario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdHorario"));

                    b.Property<TimeSpan>("HoraFin")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("HoraInicio")
                        .HasColumnType("time");

                    b.HasKey("IdHorario");

                    b.ToTable("Horarios");

                    b.HasData(
                        new
                        {
                            IdHorario = 1,
                            HoraFin = new TimeSpan(0, 17, 0, 0, 0),
                            HoraInicio = new TimeSpan(0, 8, 0, 0, 0)
                        },
                        new
                        {
                            IdHorario = 2,
                            HoraFin = new TimeSpan(0, 18, 0, 0, 0),
                            HoraInicio = new TimeSpan(0, 9, 0, 0, 0)
                        },
                        new
                        {
                            IdHorario = 3,
                            HoraFin = new TimeSpan(0, 19, 0, 0, 0),
                            HoraInicio = new TimeSpan(0, 10, 0, 0, 0)
                        });
                });

            modelBuilder.Entity("JarredsOrderHub.Models.Pago", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("FechaPago")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Monto")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("PedidoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PedidoId");

                    b.ToTable("Pagos");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.Pedidos", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Comentarios")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CuponId")
                        .HasColumnType("int");

                    b.Property<string>("EstadoPedido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaPedido")
                        .HasColumnType("datetime2");

                    b.Property<string>("MetodoPago")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CuponId");

                    b.ToTable("Pedidos");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.Platillo", b =>
                {
                    b.Property<int>("IdPlatillo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPlatillo"));

                    b.Property<bool>("Activo")
                        .HasColumnType("bit");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdCategoria")
                        .HasColumnType("int");

                    b.Property<string>("Imagen")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("IdPlatillo");

                    b.HasIndex("IdCategoria");

                    b.ToTable("Platillos");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.RecuperacionContrasenia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaExpiracion")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Utilizado")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("RecuperacionesContrasenias");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.Reporte", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DescripcionReporte")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaReporte")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdCliente")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdCliente");

                    b.ToTable("Reportes");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.Rol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Permisos")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Descripcion = "Acceso total a la pagina.",
                            Nombre = "Administrador",
                            Permisos = "Administrar usuarios, Ver empleados, Ver tareas, Administrar tareas"
                        });
                });

            modelBuilder.Entity("JarredsOrderHub.Models.SeccionContenido", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ArchivoPdf")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Contenido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Estado")
                        .HasColumnType("bit");

                    b.Property<string>("PreguntasFrecuentes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Seccion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SeccionesContenido");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.Tareas", b =>
                {
                    b.Property<int>("IdTarea")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdTarea"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("FechaEntrega")
                        .HasColumnType("datetime2");

                    b.Property<int?>("IdEmpleado")
                        .HasColumnType("int");

                    b.Property<string>("NombreTarea")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdTarea");

                    b.HasIndex("IdEmpleado");

                    b.ToTable("Tareas");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.CuponCliente", b =>
                {
                    b.HasOne("JarredsOrderHub.Models.Cupon", "Cupon")
                        .WithMany()
                        .HasForeignKey("CuponId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cupon");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.DetallePedido", b =>
                {
                    b.HasOne("JarredsOrderHub.Models.Pedidos", "Pedido")
                        .WithMany("Detalles")
                        .HasForeignKey("PedidoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JarredsOrderHub.Models.Platillo", "Platillo")
                        .WithMany()
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pedido");

                    b.Navigation("Platillo");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.Empleado", b =>
                {
                    b.HasOne("JarredsOrderHub.Models.Horario", "Horario")
                        .WithMany()
                        .HasForeignKey("IdHorario");

                    b.HasOne("JarredsOrderHub.Models.Rol", "Rol")
                        .WithMany()
                        .HasForeignKey("IdRol");

                    b.Navigation("Horario");

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.Pago", b =>
                {
                    b.HasOne("JarredsOrderHub.Models.Pedidos", "Pedido")
                        .WithMany()
                        .HasForeignKey("PedidoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pedido");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.Pedidos", b =>
                {
                    b.HasOne("JarredsOrderHub.Models.Cupon", "Cupon")
                        .WithMany()
                        .HasForeignKey("CuponId");

                    b.Navigation("Cupon");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.Platillo", b =>
                {
                    b.HasOne("JarredsOrderHub.Models.Categoria", "Categoria")
                        .WithMany("Platillos")
                        .HasForeignKey("IdCategoria");

                    b.Navigation("Categoria");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.Reporte", b =>
                {
                    b.HasOne("JarredsOrderHub.Models.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("IdCliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.Tareas", b =>
                {
                    b.HasOne("JarredsOrderHub.Models.Empleado", "Empleado")
                        .WithMany()
                        .HasForeignKey("IdEmpleado")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Empleado");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.Categoria", b =>
                {
                    b.Navigation("Platillos");
                });

            modelBuilder.Entity("JarredsOrderHub.Models.Pedidos", b =>
                {
                    b.Navigation("Detalles");
                });
#pragma warning restore 612, 618
        }
    }
}
