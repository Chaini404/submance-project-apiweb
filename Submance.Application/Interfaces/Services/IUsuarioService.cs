using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Submance.Application.DTOs.Usuario; // ✅ Agregamos el using

namespace Submance.Application.Interfaces.Services
{
    public interface IUsuarioService
    {
        // CAMBIO: UsuarioDto -> UsuarioResponseDto
        IEnumerable<UsuarioResponseDto> GetAll();

        // CAMBIO: UsuarioDto -> UsuarioResponseDto
        UsuarioResponseDto GetById(int id);

        // CAMBIO: CreateUsuarioRequest -> UsuarioRequestDto
        void Create(UsuarioRequestDto request);

        // CAMBIO: UpdateUsuarioRequest -> UsuarioRequestDto
        void Update(int id, UsuarioRequestDto request);

        void ChangeStatus(int id, bool enabled);
    }
}
