using System.Threading.Tasks;

namespace SupremaMiddleware.Server
{
    public interface IClientAccessTokenManagementService
    {
        Task<string> GetClientAccessTokenAsync(bool forceRenewal = false);
    }
}
