using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Submance.Application.DTOs.Artista; // ✅ Agregamos el using

namespace Submance.Application.Interfaces.Services
{
    public interface IArtistaService
    {
        // CAMBIO: ArtistaDto -> ArtistaResponseDto (Tu archivo real)
        IEnumerable<ArtistaResponseDto> GetAll();

        // CAMBIO: ArtistaDto -> ArtistaResponseDto
        ArtistaResponseDto GetById(int id);

        // CAMBIO: CreateArtistaRequest -> ArtistaRequestDto
        void Create(ArtistaRequestDto request);

        // CAMBIO: UpdateArtistaRequest -> ArtistaRequestDto
        void Update(int id, ArtistaRequestDto request);

        void Delete(int id);
    }
}
