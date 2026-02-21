using Dapper;
using Submance.Application.Interfaces.Security;
using Submance.Application.Interfaces.Services;
using Submance.Application.ViewModels;
using Submance.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace Submance.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly DbConnectionFactory _connectionFactory;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(DbConnectionFactory connectionFactory, IPasswordHasher passwordHasher)
        {
            _connectionFactory = connectionFactory;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> RegistrarArtistaAsync(RegistroViewModel dto)
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            string sql = @"
                WITH check_email AS (
                    SELECT 1 FROM ""Usuarios"" WHERE ""Correo"" = @Correo
                ),
                nuevo_usuario AS (
                    INSERT INTO ""Usuarios"" (""Nombre"", ""Correo"", ""Password"", ""Rol"", ""Estado"", ""FechaRegistro"") 
                    SELECT @NombreReal, @Correo, @Password, 'Artista', 'Activo', NOW()
                    WHERE NOT EXISTS (SELECT 1 FROM check_email)
                    RETURNING ""IdUsuario""
                )
                INSERT INTO ""Artistas"" (""IdUsuario"", ""NombreArtistico"", ""NombreReal"", ""Pais"", ""Estado"", ""FechaRegistro"")
                SELECT ""IdUsuario"", @NombreArtistico, @NombreReal, @Pais, TRUE, NOW()
                FROM nuevo_usuario
                RETURNING ""IdUsuario"";";

            var parameters = new
            {
                Correo = dto.Correo,
                NombreReal = dto.NombreReal,
                Password = _passwordHasher.Hash(dto.Password),
                NombreArtistico = dto.NombreArtistico,
                Pais = dto.Pais
            };

            try
            {
                var result = await connection.ExecuteScalarAsync<int?>(sql, parameters);
                return result.HasValue && result.Value > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en registro: " + ex.Message, ex);
            }
        }
    }
}