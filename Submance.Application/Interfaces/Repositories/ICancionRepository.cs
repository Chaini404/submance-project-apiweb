using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Submance.Domain.Entities;

namespace Submance.Application.Interfaces.Repositories
{
    public interface ICancionRepository
    {
        Task<IEnumerable<Cancion>> GetAllAsync();
        Task<Cancion> GetByIdAsync(int id);

        Task<IEnumerable<Cancion>> GetByArtistaAsync(int idArtista);
        Task<IEnumerable<Cancion>> GetPendientesRevisionAsync();

        Task AddAsync(Cancion cancion);
        Task UpdateAsync(Cancion cancion);
        Task DeleteAsync(int id);
    }   
}
