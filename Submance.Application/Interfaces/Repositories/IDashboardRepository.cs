using System.Threading.Tasks;

namespace Submance.Application.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<object> GetDashboardDataAsync();
    }
}