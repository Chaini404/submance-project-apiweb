using Submance.Application.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submance.Application.Services
{
    public class CancionService : ICancionService
    {
        public void Create(CreateCancionRequest request)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CancionDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CancionDto> GetByAlbum(int albumId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CancionDto> GetByGenero(int generoId)
        {
            throw new NotImplementedException();
        }

        public CancionDto GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, UpdateCancionRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
