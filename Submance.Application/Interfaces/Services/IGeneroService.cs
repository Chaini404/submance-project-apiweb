using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Submance.Application.DTOs.Genero; // ✅ Agregamos el using

namespace Submance.Application.Interfaces.Services
{
    public interface IGeneroService
    {
        // CAMBIO: GeneroDto -> GeneroResponseDto
        IEnumerable<GeneroResponseDto> GetAll();

        // CAMBIO: GeneroDto -> GeneroResponseDto
        GeneroResponseDto GetById(int id);

        // CAMBIO: CreateGeneroRequest -> GeneroRequestDto
        void Create(GeneroRequestDto request);

        // CAMBIO: UpdateGeneroRequest -> GeneroRequestDto
        void Update(int id, GeneroRequestDto request);

        void Delete(int id);
    }
}
