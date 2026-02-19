#nullable enable
using Dapper;
using Submance.Application.Interfaces.Repositories;
using Submance.Domain.Entities;
using Submance.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Submance.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DbConnectionFactory _db;
        public UsuarioRepository(DbConnectionFactory db) => _db = db;

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            return await conn.QueryAsync<Usuario>(@"SELECT * FROM ""Usuarios""");
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            return await conn.QueryFirstOrDefaultAsync<Usuario>(
                @"SELECT * FROM ""Usuarios"" WHERE ""IdUsuario"" = @Id", new { Id = id });
        }

        public async Task<Usuario?> GetByCorreoAsync(string correo)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            return await conn.QueryFirstOrDefaultAsync<Usuario>(
                @"SELECT * FROM ""Usuarios"" WHERE ""Correo"" = @Correo", new { Correo = correo });
        }

        public async Task<Usuario?> GetByUsernameAsync(string nombre)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            return await conn.QueryFirstOrDefaultAsync<Usuario>(
                @"SELECT * FROM ""Usuarios"" WHERE ""Nombre"" = @Nombre", new { Nombre = nombre });
        }

        public async Task AddAsync(Usuario usuario)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            var sql = @"INSERT INTO ""Usuarios"" (""Nombre"", ""Correo"", ""Password"", ""Rol"", ""Estado"", ""FechaRegistro"") 
                        VALUES (@Nombre, @Correo, @Password, @Rol, @Estado, NOW())";
            await conn.ExecuteAsync(sql, new { usuario.Nombre, usuario.Correo, usuario.Password, usuario.Rol, usuario.Estado });
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            var sql = @"UPDATE ""Usuarios"" SET ""Nombre"" = @Nombre, ""Correo"" = @Correo, ""Rol"" = @Rol, ""Estado"" = @Estado WHERE ""IdUsuario"" = @IdUsuario";
            await conn.ExecuteAsync(sql, new { usuario.Nombre, usuario.Correo, usuario.Rol, usuario.Estado, usuario.IdUsuario });
        }

        public async Task DeleteAsync(int id)
        {
            await using var conn = _db.CreateConnection();
            await conn.OpenAsync();
            await conn.ExecuteAsync(@"DELETE FROM ""Usuarios"" WHERE ""IdUsuario"" = @Id", new { Id = id });
        }
    }
}