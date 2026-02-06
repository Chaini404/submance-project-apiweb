Create database SubmanceProjectDb;
use SubmanceProjectDb;

CREATE TABLE Rol (
    idRol INT IDENTITY PRIMARY KEY,
    nombreRol VARCHAR(50) NOT NULL
);

CREATE TABLE Usuario (
    idUsuario INT IDENTITY PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    correo VARCHAR(100) UNIQUE NOT NULL,
    password VARCHAR(100) NOT NULL,
    idRol INT NOT NULL,
    activo BIT DEFAULT 1,
    FOREIGN KEY (idRol) REFERENCES Rol(idRol)
);

CREATE TABLE Artista (
    idArtista INT IDENTITY PRIMARY KEY,
    nombreArtistico VARCHAR(100) NOT NULL,
    nombreReal VARCHAR(100),
    correo VARCHAR(100) UNIQUE NOT NULL,
    activo BIT NOT NULL DEFAULT 1
);

CREATE TABLE Genero (
    idGenero INT IDENTITY PRIMARY KEY,
    nombreGenero VARCHAR(50) NOT NULL,
    descripcion VARCHAR(150),
	activo BIT NOT NULL DEFAULT 1,

);

CREATE TABLE Album (
    idAlbum INT IDENTITY PRIMARY KEY,
    titulo VARCHAR(100) NOT NULL,
    fechaLanzamiento DATE,
    idArtista INT NOT NULL,
	activo BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (idArtista) REFERENCES Artista(idArtista)
);

CREATE TABLE Cancion (
    idCancion INT IDENTITY PRIMARY KEY,
    titulo VARCHAR(100) NOT NULL,
    duracion TIME,
    archivo VARCHAR(200),
    idAlbum INT,
    idGenero INT NOT NULL,
    idArtista INT NOT NULL,
	activo BIT NOT NULL DEFAULT 1,
    estado VARCHAR(30) DEFAULT 'Pendiente', --Pendiente, Aprobada, Rechazada

    FOREIGN KEY (idAlbum) REFERENCES Album(idAlbum),
    FOREIGN KEY (idGenero) REFERENCES Genero(idGenero),
    FOREIGN KEY (idArtista) REFERENCES Artista(idArtista)
);

CREATE TABLE Revision (
    idRevision INT IDENTITY PRIMARY KEY,
    idCancion INT NOT NULL,
    idRevisor INT NOT NULL,
    fechaRevision DATETIME DEFAULT GETDATE(),
    observacion VARCHAR(300),
    resultado VARCHAR(30),
    FOREIGN KEY (idCancion) REFERENCES Cancion(idCancion),
    FOREIGN KEY (idRevisor) REFERENCES Usuario(idUsuario)
);


SELECT * FROM Usuario
SELECT * FROM Rol
SELECT * FROM Artista
SELECT * FROM Genero
SELECT * FROM Album
SELECT * FROM Cancion
SELECT * FROM Revision