using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Submance.Application.Interfaces.Services;
using Submance.Application.DTOs.Genero; // ✅ Conecta con tus DTOs reales



namespace Submance.Application.Services
{
    public class GeneroService : IGeneroService
    {
        public IEnumerable<GeneroResponseDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public GeneroResponseDto GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Create(GeneroRequestDto request)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, GeneroRequestDto request)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
