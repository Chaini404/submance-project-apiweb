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
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public UsuarioRepository(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            List<Usuario> usuarios = new();

            using SqlConnection con = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear la conexion SQL");

            await con.OpenAsync();

            using SqlCommand cmd = new SqlCommand("sp_Usuario_GetAll", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30;

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            int ordId = reader.GetOrdinal("idUsuario");
            int ordNombre = reader.GetOrdinal("nombre");
            int ordCorreo = reader.GetOrdinal("correo");
            int ordPassword = reader.GetOrdinal("password");
            int ordRol = reader.GetOrdinal("idRol");
            int ordActivo = reader.GetOrdinal("activo");

            while (await reader.ReadAsync())
            {
                usuarios.Add(new Usuario
                {
                    IdUsuario = reader.GetInt32(ordId),
                    Nombre = reader.GetString(ordNombre),
                    Correo = reader.GetString(ordCorreo),
                    Password = reader.GetString(ordPassword),
                    IdRol = reader.GetInt32(ordRol),
                    Activo = reader.GetBoolean(ordActivo)
                });
            }

            return usuarios;
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<Usuario?> GetByIdAsync(int id)
        {
            Usuario? usuario = null;

            using SqlConnection con = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear la conexion SQL");

            await con.OpenAsync();

            using SqlCommand cmd = new SqlCommand("sp_Usuario_GetById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = id;

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                usuario = MapUsuario(reader);
            }

            return usuario;
        }

        // =========================
        // GET BY CORREO
        // =========================
        public async Task<Usuario?> GetByCorreoAsync(string correo)
        {
            Usuario? usuario = null;

            using SqlConnection con = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear la conexion SQL");

            await con.OpenAsync();

            using SqlCommand cmd = new SqlCommand("sp_Usuario_GetByCorreo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@correo", SqlDbType.VarChar, 100).Value = correo;

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                usuario = MapUsuario(reader);
            }

            return usuario;
        }

        // =========================
        // ADD
        // =========================
        public async Task Add(Usuario usuario)
        {
            using SqlConnection con = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear la conexion SQL");

            await con.OpenAsync();

            using SqlCommand cmd = new SqlCommand("sp_Usuario_Add", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = usuario.Nombre;
            cmd.Parameters.Add("@correo", SqlDbType.VarChar, 100).Value = usuario.Correo;
            cmd.Parameters.Add("@password", SqlDbType.VarChar, 100).Value = usuario.Password;
            cmd.Parameters.Add("@idRol", SqlDbType.Int).Value = usuario.IdRol;

            await cmd.ExecuteNonQueryAsync();
        }

        // =========================
        // UPDATE
        // =========================
        public async Task Update(Usuario usuario)
        {
            using SqlConnection con = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear la conexion SQL");

            await con.OpenAsync();

            using SqlCommand cmd = new SqlCommand("sp_Usuario_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = usuario.IdUsuario;
            cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = usuario.Nombre;
            cmd.Parameters.Add("@correo", SqlDbType.VarChar, 100).Value = usuario.Correo;
            cmd.Parameters.Add("@password", SqlDbType.VarChar, 100).Value = usuario.Password;
            cmd.Parameters.Add("@idRol", SqlDbType.Int).Value = usuario.IdRol;

            await cmd.ExecuteNonQueryAsync();
        }

        // =========================
        // DELETE (SOFT DELETE)
        // =========================
        public async Task Delete(int id)
        {
            using SqlConnection con = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear la conexion SQL");

            await con.OpenAsync();

            using SqlCommand cmd = new SqlCommand("sp_Usuario_Delete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = id;

            await cmd.ExecuteNonQueryAsync();
        }

        // =========================
        // MAPPER (reutilizable)
        // =========================
        private static Usuario MapUsuario(SqlDataReader reader)
        {
            return new Usuario
            {
                IdUsuario = reader.GetInt32(reader.GetOrdinal("idUsuario")),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Correo = reader.GetString(reader.GetOrdinal("correo")),
                Password = reader.GetString(reader.GetOrdinal("password")),
                IdRol = reader.GetInt32(reader.GetOrdinal("idRol")),
                Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
            };
        }
    }
}
