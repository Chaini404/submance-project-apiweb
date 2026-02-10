using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Submance.Application.Interfaces.Services;
using Submance.Application.DTOs.Artista; // ✅ Conecta con tus DTOs reales



namespace Submance.Application.Services
{
    public class ArtistaService : IArtistaService
    {
        // Implementación vacía para que compile
        public IEnumerable<ArtistaResponseDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public ArtistaResponseDto GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Create(ArtistaRequestDto request)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, ArtistaRequestDto request)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
