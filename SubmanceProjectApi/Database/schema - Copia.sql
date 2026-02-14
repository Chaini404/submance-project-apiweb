-- 1. LIMPIEZA E INICIO
DROP DATABASE IF EXISTS SubmanceProjectDb;
CREATE DATABASE SubmanceProjectDb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE SubmanceProjectDb;

-- =============================================
-- TABLA 1: SEGURIDAD (Usuarios del sistema)
-- =============================================
CREATE TABLE Rol (
    IdRol INT AUTO_INCREMENT PRIMARY KEY,
    NombreRol VARCHAR(50) NOT NULL
);

CREATE TABLE Usuario (
    IdUsuario INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Correo VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(100) NOT NULL, 
    IdRol INT NOT NULL,
    Activo TINYINT(1) DEFAULT 1, 
    FOREIGN KEY (IdRol) REFERENCES Rol(IdRol)
);

-- =============================================
-- TABLA 2: GÉNEROS (Necesario para clasificar)
-- =============================================
CREATE TABLE Generos (
    IdGenero INT AUTO_INCREMENT PRIMARY KEY,
    NombreGenero VARCHAR(50) NOT NULL, 
    Descripcion VARCHAR(150),
    Estado TINYINT(1) DEFAULT 1
);

-- =============================================
-- TABLA 3: ARTISTAS (Ajustada a tu Formulario)
-- =============================================
CREATE TABLE Artistas (
    IdArtista INT AUTO_INCREMENT PRIMARY KEY,
    NombreArtistico VARCHAR(100) NOT NULL, -- VIENE DEL FORM
    NombreReal VARCHAR(100),               -- VIENE DEL FORM
    Correo VARCHAR(100) NOT NULL UNIQUE,   -- VIENE DEL FORM
    
    -- Pais lo dejamos OPCIONAL (DEFAULT NULL) por si algún día lo agregas
    Pais VARCHAR(100) DEFAULT NULL,        
    
    FechaRegistro DATETIME DEFAULT CURRENT_TIMESTAMP,
    Estado TINYINT(1) DEFAULT 1
);

-- =============================================
-- TABLA 4: ÁLBUMES (Opcional, para el futuro)
-- =============================================
CREATE TABLE Albumes (
    IdAlbum INT AUTO_INCREMENT PRIMARY KEY,
    Titulo VARCHAR(100) NOT NULL,
    FechaLanzamiento DATE,
    IdArtista INT NOT NULL,
    Activo TINYINT(1) DEFAULT 1,
    FOREIGN KEY (IdArtista) REFERENCES Artistas(IdArtista)
);

-- =============================================
-- TABLA 5: DEMOS (Lo que envías en el Form)
-- =============================================
CREATE TABLE Demos (
    IdCancion INT AUTO_INCREMENT PRIMARY KEY, 
    Titulo VARCHAR(100) NOT NULL,    -- VIENE DEL FORM (inputTrackTitle)
    
    Archivo VARCHAR(500),            -- VIENE DEL FORM (inputLink)
    
    -- Estos campos NO están en tu form, así que permiten NULL
    Duracion TIME DEFAULT NULL,
    IdAlbum INT DEFAULT NULL,
    
    IdGenero INT NOT NULL, -- Se asignará automáticamente el 1 en el backend
    IdArtista INT NOT NULL, -- Se crea con los datos del form
    
    Estado VARCHAR(30) DEFAULT 'Pendiente', 
    Activo TINYINT(1) DEFAULT 1,
    FechaEnvio DATETIME DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (IdAlbum) REFERENCES Albumes(IdAlbum),
    FOREIGN KEY (IdGenero) REFERENCES Generos(IdGenero),
    FOREIGN KEY (IdArtista) REFERENCES Artistas(IdArtista)
);

-- =============================================
-- TABLA 6: REVISIONES (Para cuando tú revises los demos)
-- =============================================
CREATE TABLE Revisiones (
    IdRevision INT AUTO_INCREMENT PRIMARY KEY,
    IdCancion INT NOT NULL,
    IdRevisor INT NOT NULL,
    FechaRevision DATETIME DEFAULT CURRENT_TIMESTAMP,
    Observacion VARCHAR(300),
    Resultado VARCHAR(30),
    FOREIGN KEY (IdCancion) REFERENCES Demos(IdCancion) ON DELETE CASCADE,
    FOREIGN KEY (IdRevisor) REFERENCES Usuario(IdUsuario)
);

-- =============================================
-- DATOS NECESARIOS (SEED DATA)
-- =============================================

-- 1. Roles del Sistema
INSERT INTO Rol (NombreRol) VALUES ('Administrador'), ('A&R'), ('Artista');

-- 2. Tu Usuario Admin
INSERT INTO Usuario (Nombre, Correo, Password, IdRol) VALUES 
('Jef Karlen', 'admin@submance.com', '123456', 1);

-- 3. Géneros Musicales
INSERT INTO Generos (NombreGenero, Descripcion) VALUES 
('Uplifting Trance', '138-140 BPM'),
('Progressive House', 'Deep & Melodic'),
('Techno', 'Underground');

-- 4. Artista de Prueba (SIN PAIS, porque tu form no lo pide)
INSERT INTO Artistas (NombreArtistico, NombreReal, Correo) VALUES 
('Artista Prueba', 'Juan Perez', 'prueba@email.com');

-- Verificamos que todo se creó bien
SELECT * FROM Artistas;
SELECT * FROM Demos;
SELECT * FROM Usuario;
SELECT * FROM Rol; 
SELECT * FROM Generos;    
SELECT * FROM Albumes;       
SELECT * FROM Revisiones; 