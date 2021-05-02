using SupremaMiddleware.Server.Models;
using System.Threading.Tasks;

namespace SupremaMiddleware.Server
{
    public interface IBiostarApiClient
    {
        Task<UserCollection> GetUserCollection(int? offset = 1, int? limit = 100);
    }
}
