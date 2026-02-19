#nullable enable 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Submance.Application.DTOs.Artista;
using Submance.Application.Interfaces.Repositories;
using Submance.Application.Interfaces.Services;
using Submance.Domain.Entities;

namespace Submance.Application.Services
{
    public class ArtistaService : IArtistaService
    {
        private readonly IArtistaRepository _artistaRepository;

        public ArtistaService(IArtistaRepository artistaRepository)
        {
            _artistaRepository = artistaRepository;
        }

        public async Task<IEnumerable<ArtistaResponseDto>> GetAllAsync()
        {
            var artistas = await _artistaRepository.GetAllAsync();

            return artistas.Select(a => new ArtistaResponseDto
            {
                IdArtista = a.IdArtista,
                NombreArtistico = a.NombreArtistico,
                Correo = "", // Artista ya no tiene Correo propio en BD, se deja vacío o se saca del DTO
                Estado = a.Estado // Corregido de Activo a Estado
            });
        }

        public async Task<ArtistaResponseDto?> GetByIdAsync(int id)
        {
            var artista = await _artistaRepository.GetByIdAsync(id);
            if (artista == null) return null;

            return new ArtistaResponseDto
            {
                IdArtista = artista.IdArtista,
                NombreArtistico = artista.NombreArtistico,
                Correo = "",
                Estado = artista.Estado // Corregido
            };
        }

        public async Task CreateAsync(ArtistaRequestDto request)
        {
            var entity = new Artista
            {
                NombreArtistico = request.NombreArtistico,
                NombreReal = request.NombreReal,
                // Correo eliminado ya que no existe en tabla Artistas
                Estado = true, // Corregido de Activo a Estado
                FechaRegistro = DateTime.Now
            };

            await _artistaRepository.AddAsync(entity);
        }

        public async Task UpdateAsync(int id, ArtistaRequestDto request)
        {
            var entity = new Artista
            {
                IdArtista = id,
                NombreArtistico = request.NombreArtistico,
                NombreReal = request.NombreReal
                // Correo eliminado
            };
            await _artistaRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _artistaRepository.DeleteAsync(id);
        }
    }
}