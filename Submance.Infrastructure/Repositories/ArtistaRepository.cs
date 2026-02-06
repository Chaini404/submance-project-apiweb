using Microsoft.Data.SqlClient;
using Submance.Application.Interfaces.Repositories;
using Submance.Application.Interfaces.Repository;
using Submance.Domain.Entities;
using Submance.Infrastructure.Data;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Submance.Infrastructure.Repositories
{
    public class ArtistaRepository : IArtistaRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public ArtistaRepository(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<IEnumerable<Artista>> GetAllAsync()
        {
            List<Artista> artistas = new();

            using SqlConnection con = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear la conexion SQL");

            await con.OpenAsync();

            using SqlCommand cmd = new SqlCommand("sp_Artista_GetAll", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30;

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                artistas.Add(MapArtista(reader));
            }

            return artistas;
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<Artista?> GetByIdAsync(int id)
        {
            Artista? artista = null;

            using SqlConnection con = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear la conexion SQL");

            await con.OpenAsync();

            using SqlCommand cmd = new SqlCommand("sp_Artista_GetById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@idArtista", SqlDbType.Int).Value = id;

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                artista = MapArtista(reader);
            }

            return artista;
        }

        // =========================
        // ADD
        // =========================
        public async Task AddAsync(Artista artista)
        {
            using SqlConnection con = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear la conexion SQL");

            await con.OpenAsync();

            using SqlCommand cmd = new SqlCommand("sp_Artista_Add", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@nombreArtistico", SqlDbType.VarChar, 100).Value = artista.NombreArtistico;
            cmd.Parameters.Add("@nombreReal", SqlDbType.VarChar, 100).Value = artista.NombreReal;
            cmd.Parameters.Add("@correo", SqlDbType.VarChar, 100).Value = artista.Correo;

            await cmd.ExecuteNonQueryAsync();
        }

        // =========================
        // UPDATE
        // =========================
        public async Task UpdateAsync(Artista artista)
        {
            using SqlConnection con = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear la conexion SQL");

            await con.OpenAsync();

            using SqlCommand cmd = new SqlCommand("sp_Artista_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@idArtista", SqlDbType.Int).Value = artista.IdArtista;
            cmd.Parameters.Add("@nombreArtistico", SqlDbType.VarChar, 100).Value = artista.NombreArtistico;
            cmd.Parameters.Add("@nombreReal", SqlDbType.VarChar, 100).Value = artista.NombreReal;
            cmd.Parameters.Add("@correo", SqlDbType.VarChar, 100).Value = artista.Correo;

            await cmd.ExecuteNonQueryAsync();
        }

        // =========================
        // DELETE (SOFT DELETE)
        // =========================
        public async Task DeleteAsync(int id)
        {
            using SqlConnection con = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear la conexion SQL");

            await con.OpenAsync();

            using SqlCommand cmd = new SqlCommand("sp_Artista_Delete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@idArtista", SqlDbType.Int).Value = id;

            await cmd.ExecuteNonQueryAsync();
        }

        // =========================
        // MAPPER
        // =========================
        private static Artista MapArtista(SqlDataReader reader)
        {
            return new Artista
            {
                IdArtista = reader.GetInt32(reader.GetOrdinal("idArtista")),
                NombreArtistico = reader.GetString(reader.GetOrdinal("nombreArtistico")),
                NombreReal = reader.GetString(reader.GetOrdinal("nombreReal")),
                Correo = reader.GetString(reader.GetOrdinal("correo")),
                Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
            };
        }
    }
}
