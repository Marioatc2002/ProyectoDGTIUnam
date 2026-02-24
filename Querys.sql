-- 1. TABLAS DE CATÁLOGOS BASE
CREATE TABLE "Rol"(
    "id" INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    "Rol" TEXT NOT NULL
);

CREATE TABLE "Genero"(
    "id" INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    "Tipo" TEXT NOT NULL
);

CREATE TABLE "CategoriaPostre"(
    "id" INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    "Tipo" TEXT NOT NULL
);

-- 2. NUEVA TABLA DIRECCIONES (Centralizada)
CREATE TABLE "Direccion"(
    "id" BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    "CalleNumero" TEXT NOT NULL,
    "Colonia" TEXT NOT NULL,
    "Ciudad" TEXT NOT NULL,
    "CP" VARCHAR(10) NOT NULL,
    "Latitud" DECIMAL(9, 6) NULL,
    "Longitud" DECIMAL(9, 7) NULL,
    "Referencias" TEXT NULL
);

-- 3. ENTIDADES PRINCIPALES
CREATE TABLE "Usuario"(
    "id" BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    "Nombre" TEXT NOT NULL,
    "ApellidoPaterno" TEXT NOT NULL,
    "ApellidoMaterno" TEXT NOT NULL,
    "SegundoNombre" TEXT NULL,
    "Email" TEXT NOT NULL UNIQUE,
    "Password" TEXT NOT NULL,
    "Salt" TEXT NOT NULL,
    "Status" CHAR(20) NOT NULL,
    "RolId" INT NOT NULL,
    "GeneroId" INT NOT NULL,
    "FechaAlta" DATETIME NOT NULL DEFAULT GETDATE(),
    "FechaNacimiento" DATE NOT NULL,
    "Telefono" TEXT NOT NULL,
    "UrlImagen" TEXT NULL,
    CONSTRAINT "fk_usuario_rol" FOREIGN KEY("RolId") REFERENCES "Rol"("id"),
    CONSTRAINT "fk_usuario_genero" FOREIGN KEY("GeneroId") REFERENCES "Genero"("id")
);

CREATE TABLE "Cliente"(
    "id" BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    "Nombre" TEXT NOT NULL,
    "ApellidoPaterno" TEXT NOT NULL,
    "ApellidoMaterno" TEXT NOT NULL,
    "RFC" VARCHAR(15) NULL,
    "Email" TEXT NOT NULL,
    "Telefono" TEXT NOT NULL,
    "DireccionId" BIGINT NULL,
    CONSTRAINT "fk_cliente_direccion" FOREIGN KEY("DireccionId") REFERENCES "Direccion"("id")
);

CREATE TABLE "Proveedores"(
    "id" INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    "NombreEmpresa" TEXT NOT NULL,
    "ContactoNombre" TEXT NOT NULL,
    "Telefono" TEXT NOT NULL,
    "DireccionId" BIGINT NOT NULL,
    "UrlImagen" TEXT NULL,
    CONSTRAINT "fk_proveedor_direccion" FOREIGN KEY("DireccionId") REFERENCES "Direccion"("id")
);

-- 4. PRODUCTOS E INVENTARIO
CREATE TABLE "ProductoPerecedero"(
    "IdCodigo" BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    "Nombre" TEXT NOT NULL,
    "Descripcion" TEXT NOT NULL,
    "FechaEntrada" DATE NOT NULL,
    "FechaVencimiento" DATE NOT NULL,
    "CantidadStock" INT NOT NULL,
    "UnidadMedida" VARCHAR(50) NOT NULL,
    "UrlImagen" TEXT NULL
);

CREATE TABLE "ProductoProveedor"(
    "id" INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    "IdProductoCodigo" BIGINT NOT NULL,
    "IdProveedor" INT NOT NULL,
    "CostoUnidad" DECIMAL(10, 2) NOT NULL,
    "UnidadMedida" VARCHAR(50) NOT NULL,
    "CantidadUnidad" BIGINT NOT NULL,
    CONSTRAINT "fk_prodprov_producto" FOREIGN KEY("IdProductoCodigo") REFERENCES "ProductoPerecedero"("IdCodigo"),
    CONSTRAINT "fk_prodprov_proveedor" FOREIGN KEY("IdProveedor") REFERENCES "Proveedores"("id")
);

-- 5. RECETAS Y POSTRES
CREATE TABLE "ProductoPostre"(
    "IdProducto" INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    "Nombre" TEXT NOT NULL,
    "FechaCreacion" DATE NOT NULL,
    "IdCategoriaProducto" INT NOT NULL,
    "UrlImagen" TEXT NULL,
    CONSTRAINT "fk_postre_categoria" FOREIGN KEY("IdCategoriaProducto") REFERENCES "CategoriaPostre"("id")
);

CREATE TABLE "ProductoReceta"(
    "id" INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    "IdProductoPostre" INT NOT NULL,
    "IdIngrediente" BIGINT NOT NULL,
    "Cantidad" DECIMAL(10, 2) NOT NULL,
    "UnidadMedida" VARCHAR(50) NOT NULL,
    CONSTRAINT "fk_receta_postre" FOREIGN KEY("IdProductoPostre") REFERENCES "ProductoPostre"("IdProducto"),
    CONSTRAINT "fk_receta_ingrediente" FOREIGN KEY("IdIngrediente") REFERENCES "ProductoPerecedero"("IdCodigo")
);

CREATE TABLE "Pasos"(
    "id" INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    "IdProducto" INT NOT NULL,
    "NumeroPaso" INT NOT NULL,
    "Descripcion" TEXT NOT NULL,
    CONSTRAINT "fk_pasos_postre" FOREIGN KEY("IdProducto") REFERENCES "ProductoPostre"("IdProducto")
);

-- 6. VENTAS Y LOGÍSTICA
CREATE TABLE "Venta"(
    "id" BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    "IdUsuario" BIGINT NOT NULL,
    "IdCliente" BIGINT NOT NULL,
    "FechaVenta" DATETIME NOT NULL DEFAULT GETDATE(),
    "Total" DECIMAL(12, 2) NOT NULL,
    "Status" VARCHAR(50) NOT NULL,
    CONSTRAINT "fk_venta_usuario" FOREIGN KEY("IdUsuario") REFERENCES "Usuario"("id"),
    CONSTRAINT "fk_venta_cliente" FOREIGN KEY("IdCliente") REFERENCES "Cliente"("id")
);

CREATE TABLE "DetalleVenta"(
    "id" BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    "IdVenta" BIGINT NOT NULL,
    "IdProductoPostre" INT NOT NULL,
    "Cantidad" INT NOT NULL,
    "PrecioVenta" DECIMAL(10, 2) NOT NULL,
    CONSTRAINT "fk_detalle_venta" FOREIGN KEY("IdVenta") REFERENCES "Venta"("id"),
    CONSTRAINT "fk_detalle_producto" FOREIGN KEY("IdProductoPostre") REFERENCES "ProductoPostre"("IdProducto")
);

CREATE TABLE "Entrega"(
    "id" BIGINT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    "IdVenta" BIGINT NOT NULL,
    "IdUsuarioRepartidor" BIGINT NOT NULL,
    "DireccionEntregaId" BIGINT NOT NULL,
    "EstatusEntrega" VARCHAR(50) NOT NULL,
    "FechaSalida" DATETIME NULL,
    "FechaEntrega" DATETIME NULL,
    "EvidenciaUrl" TEXT NULL,
    CONSTRAINT "fk_entrega_venta" FOREIGN KEY("IdVenta") REFERENCES "Venta"("id"),
    CONSTRAINT "fk_entrega_repartidor" FOREIGN KEY("IdUsuarioRepartidor") REFERENCES "Usuario"("id"),
    CONSTRAINT "fk_entrega_direccion" FOREIGN KEY("DireccionEntregaId") REFERENCES "Direccion"("id")
);