#nullable enable
using Npgsql;      // Para Connection, Command y DataReader
using NpgsqlTypes; // Para NpgsqlDbType
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
        public UsuarioRepository(DbConnectionFactory dbFactory) => _dbConnectionFactory = dbFactory;

        public async Task<Usuario?> GetByCorreoAsync(string correo)
        {
            Usuario? usuario = null;
            // BIEN: Usamos NpgsqlConnection para la conexión
            using (var con = _dbConnectionFactory.CreateConnection() as NpgsqlConnection)
            {
                if (con == null) return null;
                await con.OpenAsync(); // BIEN: NpgsqlConnection sí tiene OpenAsync

                string sql = @"SELECT ""IdUsuario"", ""Nombre"", ""Correo"", ""Password"", ""Rol"", ""Estado"" 
                               FROM ""Usuarios"" WHERE ""Correo"" = @correo";

                // BIEN: Usamos NpgsqlCommand para ejecutar el SQL
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@correo", correo); // BIEN: NpgsqlCommand tiene Parameters

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync()) usuario = MapUsuario(reader);
                    }
                }
            }
            return usuario;
        }

        public async Task Add(Usuario usuario)
        {
            using (var con = _dbConnectionFactory.CreateConnection() as NpgsqlConnection)
            {
                if (con == null) return;
                await con.OpenAsync();

                string sql = @"INSERT INTO ""Usuarios"" (""Nombre"", ""Correo"", ""Password"", ""Rol"", ""Estado"", ""FechaRegistro"") 
                               VALUES (@nombre, @correo, @password, @rol, @estado, NOW())";

                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@correo", usuario.Correo);
                    cmd.Parameters.AddWithValue("@password", usuario.Password);
                    cmd.Parameters.AddWithValue("@rol", usuario.Rol);
                    cmd.Parameters.AddWithValue("@estado", usuario.Activo ? "Activo" : "Inactivo");

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            var usuarios = new List<Usuario>();
            using (var con = _dbConnectionFactory.CreateConnection() as NpgsqlConnection)
            {
                if (con == null) return usuarios;
                await con.OpenAsync();

                string sql = @"SELECT ""IdUsuario"", ""Nombre"", ""Correo"", ""Password"", ""Rol"", ""Estado"" FROM ""Usuarios""";
                using (var cmd = new NpgsqlCommand(sql, con))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync()) usuarios.Add(MapUsuario(reader));
                }
            }
            return usuarios;
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            Usuario? usuario = null;
            using (var con = _dbConnectionFactory.CreateConnection() as NpgsqlConnection)
            {
                if (con == null) return null;
                await con.OpenAsync();
                string sql = @"SELECT * FROM ""Usuarios"" WHERE ""IdUsuario"" = @id";
                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync()) usuario = MapUsuario(reader);
                    }
                }
            }
            return usuario;
        }

        public async Task Update(Usuario usuario) => await Task.CompletedTask;
        public async Task Delete(int id) => await Task.CompletedTask;
        public async Task<Usuario?> GetByUsernameAsync(string nombre) => null;

        private static Usuario MapUsuario(NpgsqlDataReader reader)
        {
            return new Usuario
            {
                IdUsuario = reader.GetInt32(reader.GetOrdinal("IdUsuario")),
                Nombre = reader["Nombre"].ToString() ?? "",
                Correo = reader["Correo"].ToString() ?? "",
                Password = reader["Password"].ToString() ?? "",
                Rol = reader["Rol"].ToString() ?? "Usuario",
                Activo = reader["Estado"].ToString() == "Activo"
            };
        }
    }
}