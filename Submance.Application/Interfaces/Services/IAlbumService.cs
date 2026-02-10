using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Submance.Application.DTOs.Album; // ✅ IMPORTANTE: Para encontrar tus archivos reales

namespace Submance.Application.Interfaces.Services
{
    public interface IAlbumService
    {
        // CAMBIO: AlbumDto -> AlbumResponseDto
        IEnumerable<AlbumResponseDto> GetAll();

        // CAMBIO: AlbumDto -> AlbumResponseDto
        AlbumResponseDto GetById(int id);

        // CAMBIO: AlbumDto -> AlbumResponseDto
        IEnumerable<AlbumResponseDto> GetByArtista(int artistaId);

        // CAMBIO: CreateAlbumRequest -> AlbumRequestDto
        void Create(AlbumRequestDto request);

        // CAMBIO: UpdateAlbumRequest -> AlbumRequestDto
        void Update(int id, AlbumRequestDto request);

        void Delete(int id);
    }
}