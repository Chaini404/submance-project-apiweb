using Submance.Application.Interfaces.Repositories;
using Submance.Domain.Entities;
using Submance.Infrastructure.Data;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Submance.Infrastructure.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public AlbumRepository(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Album>> GetAllAsync()
        {
            var albums = new List<Album>();

            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear SqlConnection");

            using var command = new SqlCommand("sp_Album_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                albums.Add(new Album
                {
                    IdAlbum = (int)reader["idAlbum"],
                    Titulo = reader["titulo"].ToString(),
                    FechaLanzamiento = (DateTime)reader["fechaLanzamiento"],
                    IdArtista = (int)reader["idArtista"]
                });
            }

            return albums;
        }

        public async Task<Album> GetByIdAsync(int id)
        {
            Album album = null;

            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear SqlConnection");

            using var command = new SqlCommand("sp_Album_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@idAlbum", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                album = new Album
                {
                    IdAlbum = (int)reader["idAlbum"],
                    Titulo = reader["titulo"].ToString(),
                    FechaLanzamiento = (DateTime)reader["fechaLanzamiento"],
                    IdArtista = (int)reader["idArtista"]
                };
            }

            return album;
        }

        public async Task<IEnumerable<Album>> GetByArtistaAsync(int idArtista)
        {
            var albums = new List<Album>();

            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear SqlConnection");

            using var command = new SqlCommand("sp_Album_GetByArtista", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@idArtista", idArtista);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                albums.Add(new Album
                {
                    IdAlbum = (int)reader["idAlbum"],
                    Titulo = reader["titulo"].ToString(),
                    FechaLanzamiento = (DateTime)reader["fechaLanzamiento"],
                    IdArtista = (int)reader["idArtista"]
                });
            }

            return albums;
        }

        public async Task AddAsync(Album album)
        {
            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
    ?? throw new InvalidOperationException("No se pudo crear SqlConnection");


            using var command = new SqlCommand("sp_Album_Add", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@titulo", album.Titulo);
            command.Parameters.AddWithValue("@fechaLanzamiento", album.FechaLanzamiento);
            command.Parameters.AddWithValue("@idArtista", album.IdArtista);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Album album)
        {
            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear SqlConnection");

            using var command = new SqlCommand("sp_Album_Update", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@idAlbum", album.IdAlbum);
            command.Parameters.AddWithValue("@titulo", album.Titulo);
            command.Parameters.AddWithValue("@fechaLanzamiento", album.FechaLanzamiento);
            command.Parameters.AddWithValue("@idArtista", album.IdArtista);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear SqlConnection");

            using var command = new SqlCommand("sp_Album_Delete", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@idAlbum", id);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
