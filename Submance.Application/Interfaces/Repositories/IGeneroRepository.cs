using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Submance.Domain.Entities;

namespace Submance.Application.Interfaces.Repositories
{
    public interface IGeneroRepository
    {
        Task<IEnumerable<Genero>> GetAllAsync();
        Task<Genero> GetByIdAsync(int id);

        Task AddAsync(Genero genero);
        Task UpdateAsync(Genero genero);
        Task DeleteAsync(int id);
    }
}
