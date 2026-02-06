using Microsoft.Data.SqlClient;
using Submance.Application.Interfaces.Repositories;
using Submance.Domain.Entities;
using Submance.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Infrastructure.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public RolRepository(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Rol>> GetAllAsync()
        {
            List<Rol> rols = new List<Rol>();

            using (SqlConnection con = _dbConnectionFactory.CreateConnection() as SqlConnection
                   ?? throw new InvalidOperationException("No se pudo crear la conexión SQL"))
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("sp_Rol_GetAll", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 30; // buena práctica

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        int ordIdRol = reader.GetOrdinal("idRol");
                        int ordNombreRol = reader.GetOrdinal("nombreRol");

                        while (await reader.ReadAsync())
                        {
                            Rol rol = new Rol
                            {
                                IdRol = !reader.IsDBNull(ordIdRol) ? reader.GetInt32(ordIdRol) : 0,
                                NombreRol = !reader.IsDBNull(ordNombreRol) ? reader.GetString(ordNombreRol) : string.Empty
                            };

                            rols.Add(rol);
                        }
                    }
                }
            }

            return rols;
        }


        public async Task<Rol?> GetById(int id)
        {
            Rol? rol = null;

            using (SqlConnection con = _dbConnectionFactory.CreateConnection() as SqlConnection
                ?? throw new InvalidOperationException("No se pudo crear la conexion Sql"))
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("sp_Rol_GetById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 30;

                    // Mejor que AddWithValue (evita problemas de tipo)
                    cmd.Parameters.Add("@idRol", SqlDbType.Int).Value = id;

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        int ordIdRol = reader.GetOrdinal("idRol");
                        int ordNombreRol = reader.GetOrdinal("nombreRol");

                        if (await reader.ReadAsync())   // ← solo 1 registro
                        {
                            rol = new Rol
                            {
                                IdRol = !reader.IsDBNull(ordIdRol) ? reader.GetInt32(ordIdRol) : 0,
                                NombreRol = !reader.IsDBNull(ordNombreRol) ? reader.GetString(ordNombreRol) : string.Empty
                            };
                        }
                    }
                }
            }

            return rol;
        }

    }
}
