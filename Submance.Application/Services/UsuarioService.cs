using Submance.Application.Interfaces.Services;
using Submance.Application.DTOs.Usuario;
using Submance.Application.Interfaces.Repository; // 👈 OJO: Ahora usamos 'Repository' (singular)
using Submance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Submance.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public void Create(UsuarioRequestDto request)
        {
            var usuario = new Usuario
            {
                Nombre = request.Nombre,
                Correo = request.Correo,
                Password = request.Password,
                IdRol = request.IdRol,
                Activo = true
            };

            // CORRECCIÓN 1: Usamos 'Add' y le ponemos '.Wait()' porque es una Task
            _usuarioRepository.Add(usuario).Wait();
        }

        public IEnumerable<UsuarioResponseDto> GetAll()
        {
            // CORRECCIÓN 2: Usamos 'GetAllAsync' y '.Result' para obtener los datos
            var usuarios = _usuarioRepository.GetAllAsync().Result;

            var response = new List<UsuarioResponseDto>();

            // Verificamos que no sea nulo para evitar errores
            if (usuarios != null)
            {
                foreach (var u in usuarios)
                {
                    response.Add(new UsuarioResponseDto
                    {
                        IdUsuario = u.IdUsuario,
                        Nombre = u.Nombre,
                        Correo = u.Correo,
                        Rol = u.IdRol.ToString(),
                        Estado = u.Activo
                    });
                }
            }
            return response;
        }

        public UsuarioResponseDto GetById(int id)
        {
            // CORRECCIÓN 3: Usamos GetByIdAsync
            var u = _usuarioRepository.GetByIdAsync(id).Result;

            if (u == null) return null;

            return new UsuarioResponseDto
            {
                IdUsuario = u.IdUsuario,
                Nombre = u.Nombre,
                Correo = u.Correo,
                Rol = u.IdRol.ToString(),
                Estado = u.Activo
            };
        }

        public void Update(int id, UsuarioRequestDto request)
        {
            throw new NotImplementedException();
        }

        public void ChangeStatus(int id, bool enabled)
        {
            throw new NotImplementedException();
        }
    }
}