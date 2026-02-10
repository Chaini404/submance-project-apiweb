using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Submance.Application.Interfaces.Services;
using Submance.Application.DTOs.Album; // ✅ Conectamos con tus DTOs reales



namespace Submance.Application.Services
{
    public class AlbumService : IAlbumService
    {
        // Implementación provisional para que compile (luego le pondrás la lógica real)

        public IEnumerable<AlbumResponseDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public AlbumResponseDto GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AlbumResponseDto> GetByArtista(int artistaId)
        {
            throw new NotImplementedException();
        }

        public void Create(AlbumRequestDto request)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, AlbumRequestDto request)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}