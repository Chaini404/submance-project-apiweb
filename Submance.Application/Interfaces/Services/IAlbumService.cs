using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Application.Interfaces.Services
{
    public interface IAlbumService
    {
        IEnumerable<AlbumDto> GetAll();
        AlbumDto GetById(int id);
        IEnumerable<AlbumDto> GetByArtista(int artistaId);

        void Create(CreateAlbumRequest request);
        void Update(int id, UpdateAlbumRequest request);
        void Delete(int id);
    }

}
