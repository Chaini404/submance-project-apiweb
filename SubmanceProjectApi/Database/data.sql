-- 1. Crear Base de Datos (Si no existe)
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'SubmanceProjectDb')
BEGIN
    CREATE DATABASE SubmanceProjectDb;
END
GO

USE SubmanceProjectDb;
GO

-- 2. Crear Tabla Demos
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Demos]') AND type in (N'U'))
BEGIN
    CREATE TABLE Demos (
        IdDemo INT IDENTITY(1,1) PRIMARY KEY,
        TituloDemo NVARCHAR(100) NULL,
        NombreArtistico NVARCHAR(100) NULL,
        Estilo NVARCHAR(50) NULL,
        LinkDemo NVARCHAR(MAX) NULL,
        Email NVARCHAR(100) NULL,
        Estado NVARCHAR(50) DEFAULT 'Pendiente',
        FechaEnvio DATETIME DEFAULT GETDATE()
    );
END
GO

-- 3. Limpiar tabla (Opcional, para empezar limpio)
TRUNCATE TABLE Demos;
GO

-- 4. Insertar Datos de Prueba (Sin Nulos)
INSERT INTO Demos (TituloDemo, NombreArtistico, Estilo, LinkDemo, Email, Estado, FechaEnvio)
VALUES 
('Test Track', 'Jef Karlen', 'Uplifting', 'https://soundcloud.com/jefkarlen', 'jef@submance.com', 'Pendiente', GETDATE()),
('Love', 'Sounemot', 'Uplifting Trance', 'https://soundcloud.com/sounemot', 'demo@sounemot.com', 'Pendiente', GETDATE()),
('Sky Is Falling', 'Arrgic', 'Vocal Trance', 'https://soundcloud.com/arrgic', 'contact@arrgic.com', 'Pendiente', GETDATE()),
('Galaxy', 'Nova', 'Progressive', 'https://soundcloud.com/nova', 'nova@music.com', 'Aprobada', GETDATE()),
('Ocean Drive', 'RetroVision', 'House', 'https://soundcloud.com/retro', 'retro@house.com', 'Pendiente', GETDATE()),
('Silence', 'Delerium', 'Classic Trance', 'https://soundcloud.com/delerium', 'info@delerium.com', 'Rechazada', GETDATE()),
('Children', 'Robert Miles', 'Dream Trance', 'https://soundcloud.com/robertmiles', 'robert@miles.com', 'Pendiente', GETDATE()),
('Sandstorm', 'Darude', 'Trance', 'https://soundcloud.com/darude', 'darude@sandstorm.com', 'Pendiente', GETDATE());
GO

-- Ver el resultado
SELECT * FROM Demos;