Create database NotRealCoverWeb;
---usuario
CREATE TABLE Usuario (
    Id INT PRIMARY  KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50),
    Email VARCHAR(100) NOT NULL,
    Password VARCHAR(50) NOT NULL,
    Estatus TINYINT NOT NULL,
    Rol VARCHAR(255) NOT NULL
);

---FacturaVentaAlbum
CREATE TABLE FacturaVenta(
    Id INT PRIMARY KEY IDENTITY(1,1),
    FechaVenta DATE NOT NULL,
    Correlativo VARCHAR(20) NOT NULL,
    Cliente VARCHAR(100) NOT NULL,
    TotalVenta DECIMAL(10,2) NOT NULL
);
GO

----DetFacturaVentaAlbum
CREATE TABLE DetFacturaVenta(
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdFacturaVenta INT NOT NULL,
    Album VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(500) NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (IdFacturaVenta) REFERENCES FacturaVenta(Id) ON DELETE CASCADE
);
GO