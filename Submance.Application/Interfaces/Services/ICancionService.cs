using Submance.Application.DTOs.Cancion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Application.Interfaces.Services // (Ojo: Asegúrate que el namespace coincida con tu carpeta)
{
    public interface ICancionService
    {
        // 1. CAMBIO: De 'CancionDto' a 'CancionResponseDto' (Tu archivo real)
        IEnumerable<CancionResponseDto> GetAll();

        // 2. CAMBIO: De 'CancionDto' a 'CancionResponseDto'
        CancionResponseDto GetById(int id);

        // 3. CAMBIO: De 'CancionDto' a 'CancionResponseDto'
        IEnumerable<CancionResponseDto> GetByAlbum(int albumId);
        IEnumerable<CancionResponseDto> GetByGenero(int generoId);

        // 4. Este ya estaba bien (CancionRequestDto existe)
        void Create(CancionRequestDto request);

        // 5. CAMBIO: De 'UpdateCancionRequest' a 'CancionRequestDto'
        // (Porque no tienes un archivo 'UpdateCancionRequest', reutilizamos el Request normal)
        void Update(int id, CancionRequestDto request);

        void Delete(int id);
    }
}