using System.Threading.Tasks;
using TaylorFerens_Cymax_Application.Models;

namespace TaylorFerens_Cymax_Application.Interfaces
{
    public interface IShippingService
    {
        Task<Shipping> DetermineBestShipping(Shipping shipping);
    }
}
