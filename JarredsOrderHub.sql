-----SQL JARREDS ORDER HUB-----

Create Database JarredsOrderHub;
GO

Use JarredsOrderHub;
Go

--Rol--
CREATE TABLE Rol (
    idRol INT PRIMARY KEY,
    nombreRol VARCHAR(50) NOT NULL
);

--Horario--
CREATE TABLE Horario (
    idHorario INT PRIMARY KEY,
    HoraInicio TIME NOT NULL,
    HoraFin TIME NOT NULL
);

--Empleado--
CREATE TABLE Empleado (
    idEmpleado INT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    apellido VARCHAR(50) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    idRol INT,
    idHorario INT,
    salario INT,
    contrasenia VARCHAR(255) NOT NULL,
    FOREIGN KEY (idRol) REFERENCES Rol(idRol),
	FOREIGN KEY (idHorario) REFERENCES Horario(idHorario)
);

--Tarea--
CREATE TABLE Tareas (
    idTarea INT PRIMARY KEY,
    NombreTarea VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(255),
    idEmpleado INT,
    FOREIGN KEY (idEmpleado) REFERENCES Empleado(idEmpleado)
);

--Cliente--
CREATE TABLE Cliente (
    idCliente INT PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Contrasenia VARCHAR(255) NOT NULL
);

--Categoria--
CREATE TABLE Categoria (
    idCategoria INT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(255),
    Activa BIT NOT NULL
);

--Platillo--
CREATE TABLE Platillo (
    idPlatillo INT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(255),
    Precio INT NOT NULL,
    Imagen VARCHAR(255),
    idCategoria INT,
    Disponible BIT NOT NULL,
    FOREIGN KEY (idCategoria) REFERENCES Categoria(idCategoria)
);

--Pedido--
CREATE TABLE Pedido (
    idPedido INT PRIMARY KEY,
    idCliente INT,
    FechaPedido DATE NOT NULL,
    Estado VARCHAR(50) NOT NULL,
    TotalPedido DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (idCliente) REFERENCES Cliente(idCliente)
);

--Detalle Pedido--
CREATE TABLE DetallePedido (
    idDetallePedido INT PRIMARY KEY,
    idPedido INT,
    idPlatillo INT,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (idPedido) REFERENCES Pedido(idPedido),
    FOREIGN KEY (idPlatillo) REFERENCES Platillo(idPlatillo)
);


--Envio-
CREATE TABLE Envios (
    idEnvio INT PRIMARY KEY,
    idPedido INT,
    DireccionEnvio VARCHAR(255) NOT NULL,
    FechaEnvio DATE NOT NULL,
    Estado VARCHAR(50) NOT NULL,
    FOREIGN KEY (idPedido) REFERENCES Pedido(idPedido)
);

--Factura--
CREATE TABLE Factura (
    idFactura INT PRIMARY KEY,
    idPedido INT,
    FechaFactura DATE NOT NULL,
    Monto INT NOT NULL,
    EstadoFactura VARCHAR(50) NOT NULL,
    FOREIGN KEY (idPedido) REFERENCES Pedido(idPedido)
);


--ESTABLECER MODELO 
--HOSTING (INVESTIGAR)
--U RELACION CON AZURE