using Submance.Application.DTOs.Album;
using Submance.Application.Interfaces.Repositories;
using Submance.Application.Interfaces.Services;
using Submance.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Submance.Application.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;

        public AlbumService(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }

        public async Task<IEnumerable<AlbumResponseDto>> GetAllAsync()
        {
            var albumes = await _albumRepository.GetAllAsync();
            return albumes.Select(a => new AlbumResponseDto
            {
                IdAlbum = a.IdAlbum,
                Titulo = a.Titulo,
                FechaLanzamiento = a.FechaLanzamiento,
                Artista = string.Empty
            });
        }

        public async Task<AlbumResponseDto?> GetByIdAsync(int id)
        {
            var a = await _albumRepository.GetByIdAsync(id);
            if (a == null) return null;

            return new AlbumResponseDto
            {
                IdAlbum = a.IdAlbum,
                Titulo = a.Titulo,
                FechaLanzamiento = a.FechaLanzamiento,
                Artista = string.Empty
            };
        }

        public async Task<IEnumerable<AlbumResponseDto>> GetByArtistaAsync(int idArtista)
        {
            var albumes = await _albumRepository.GetByArtistaAsync(idArtista);
            return albumes.Select(a => new AlbumResponseDto
            {
                IdAlbum = a.IdAlbum,
                Titulo = a.Titulo,
                FechaLanzamiento = a.FechaLanzamiento,
                Artista = string.Empty
            });
        }

        public async Task CreateAsync(AlbumRequestDto request)
        {
            var entity = new Album
            {
                Titulo = request.Titulo,
                FechaLanzamiento = request.FechaLanzamiento,
                IdArtista = request.IdArtista
            };
            await _albumRepository.AddAsync(entity);
        }

        public async Task UpdateAsync(int id, AlbumRequestDto request)
        {
            var entity = new Album
            {
                IdAlbum = id,
                Titulo = request.Titulo,
                FechaLanzamiento = request.FechaLanzamiento,
                IdArtista = request.IdArtista
            };
            await _albumRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _albumRepository.DeleteAsync(id);
        }
    }
}