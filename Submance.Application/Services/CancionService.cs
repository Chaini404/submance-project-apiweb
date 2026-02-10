using Submance.Application.DTOs.Album;
using Submance.Application.DTOs.Artista;
using Submance.Application.DTOs.Cancion; // ✅ Aquí están tus DTOs reales
using Submance.Application.DTOs.Genero;
using Submance.Application.Interfaces.Service;
using Submance.Application.Interfaces.Services; // Asegúrate de que sea .Services (singular o plural según tu carpeta)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Application.Services
{
    public class CancionService : ICancionService
    {
        // CAMBIO 1: CreateCancionRequest -> CancionRequestDto
        public void Create(CancionRequestDto request)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        // CAMBIO 2: CancionDto -> CancionResponseDto
        public IEnumerable<CancionResponseDto> GetAll()
        {
            throw new NotImplementedException();
        }

        // CAMBIO 3: CancionDto -> CancionResponseDto
        public IEnumerable<CancionResponseDto> GetByAlbum(int albumId)
        {
            throw new NotImplementedException();
        }

        // CAMBIO 4: CancionDto -> CancionResponseDto
        public IEnumerable<CancionResponseDto> GetByGenero(int generoId)
        {
            throw new NotImplementedException();
        }

        // CAMBIO 5: CancionDto -> CancionResponseDto
        public CancionResponseDto GetById(int id)
        {
            throw new NotImplementedException();
        }

        // CAMBIO 6: UpdateCancionRequest -> CancionRequestDto
        public void Update(int id, CancionRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}
