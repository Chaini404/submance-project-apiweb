using Submance.Application.ViewModels;
using System.Threading.Tasks;

namespace Submance.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<bool> RegistrarArtistaAsync(RegistroViewModel model);
    }
}