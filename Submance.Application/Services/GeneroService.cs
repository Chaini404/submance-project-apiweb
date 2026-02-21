using Submance.Application.DTOs.Genero;
using Submance.Application.Interfaces.Repositories;
using Submance.Application.Interfaces.Services;
using Submance.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Submance.Application.Services
{
    public class GeneroService : IGeneroService
    {
        private readonly IGeneroRepository _generoRepository;

        public GeneroService(IGeneroRepository generoRepository)
        {
            _generoRepository = generoRepository;
        }

        public async Task<IEnumerable<GeneroResponseDto>> GetAllAsync()
        {
            var generos = await _generoRepository.GetAllAsync();
            return generos.Select(g => new GeneroResponseDto
            {
                IdGenero = g.IdGenero,
                NombreGenero = g.NombreGenero
            });
        }

        public async Task<GeneroResponseDto?> GetByIdAsync(int id)
        {
            var g = await _generoRepository.GetByIdAsync(id);
            if (g == null) return null;

            return new GeneroResponseDto
            {
                IdGenero = g.IdGenero,
                NombreGenero = g.NombreGenero
            };
        }

        public async Task CreateAsync(GeneroRequestDto request)
        {
            var entity = new Genero
            {
                NombreGenero = request.NombreGenero,
                Descripcion = request.Descripcion ?? string.Empty
            };
            await _generoRepository.AddAsync(entity);
        }

        public async Task UpdateAsync(int id, GeneroRequestDto request)
        {
            var entity = new Genero
            {
                IdGenero = id,
                NombreGenero = request.NombreGenero,
                Descripcion = request.Descripcion ?? string.Empty
            };
            await _generoRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _generoRepository.DeleteAsync(id);
        }
    }
}
