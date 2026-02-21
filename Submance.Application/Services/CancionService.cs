using Submance.Application.DTOs.Cancion;
using Submance.Application.Interfaces.Repositories;
using Submance.Application.Interfaces.Services;
using Submance.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Submance.Application.Services
{
    public class CancionService : ICancionService
    {
        private readonly ICancionRepository _cancionRepository;

        public CancionService(ICancionRepository cancionRepository)
        {
            _cancionRepository = cancionRepository;
        }

        public async Task<IEnumerable<CancionResponseDto>> GetAllAsync()
        {
            var canciones = await _cancionRepository.GetAllAsync();
            return canciones.Select(c => new CancionResponseDto
            {
                IdCancion = c.IdDemo,
                Titulo = c.Titulo,
                Estado = c.Estado,
                Artista = string.Empty,
                Genero = string.Empty,
                Album = string.Empty
            });
        }

        public async Task<CancionResponseDto?> GetByIdAsync(int id)
        {
            var c = await _cancionRepository.GetByIdAsync(id);
            if (c == null) return null;

            return new CancionResponseDto
            {
                IdCancion = c.IdDemo,
                Titulo = c.Titulo,
                Estado = c.Estado,
                Artista = string.Empty,
                Genero = string.Empty,
                Album = string.Empty
            };
        }

        public async Task<IEnumerable<CancionResponseDto>> GetByArtistaAsync(int idArtista)
        {
            var canciones = await _cancionRepository.GetByArtistaAsync(idArtista);
            return canciones.Select(c => new CancionResponseDto
            {
                IdCancion = c.IdDemo,
                Titulo = c.Titulo,
                Estado = c.Estado,
                Artista = string.Empty,
                Genero = string.Empty,
                Album = string.Empty
            });
        }

        public async Task CreateAsync(CancionRequestDto request)
        {
            var entity = new Cancion
            {
                Titulo = request.Titulo,
                UrlAudio = request.Archivo ?? string.Empty,
                IdArtista = request.IdArtista,
                IdGenero = request.IdGenero,
                Estado = "Pendiente",
                FechaEnvio = System.DateTime.UtcNow
            };

            await _cancionRepository.AddAsync(entity);
        }

        public async Task UpdateAsync(int id, CancionRequestDto request)
        {
            var entity = new Cancion
            {
                IdDemo = id,
                Titulo = request.Titulo,
                UrlAudio = request.Archivo ?? string.Empty,
                IdArtista = request.IdArtista,
                IdGenero = request.IdGenero
            };

            await _cancionRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _cancionRepository.DeleteAsync(id);
        }
    }
}
