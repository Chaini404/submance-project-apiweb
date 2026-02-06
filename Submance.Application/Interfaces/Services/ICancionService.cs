using Submance.Application.DTOs.Cancion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Application.Interfaces.Service
{
    public interface ICancionService
    {
        IEnumerable<CancionDto> GetAll();
        CancionDto GetById(int id);

        IEnumerable<CancionDto> GetByAlbum(int albumId);
        IEnumerable<CancionDto> GetByGenero(int generoId);

        void Create(CancionRequestDto request);
        void Update(int id, UpdateCancionRequest request);
        void Delete(int id);
    }

}
