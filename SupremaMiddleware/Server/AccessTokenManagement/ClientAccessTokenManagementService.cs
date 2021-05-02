using ConsoleApp1.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SupremaMiddleware.Server.AccessTokenManagement;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SupremaMiddleware.Server
{
    public class ClientAccessTokenManagementService : IClientAccessTokenManagementService
    {
        private const string BsSessionId = "bsSessionId";

        private readonly IHttpClientFactory _clientFactory;
        private readonly IMemoryCache _cache;
        private readonly User _user;
        private readonly ILogger<ClientAccessTokenManagementService> _logger;       

        public ClientAccessTokenManagementService(IHttpClientFactory clientFactory,
            IMemoryCache distributedCache,
            IOptions<User> user,
            ILogger<ClientAccessTokenManagementService> logger)
        {
            _clientFactory = clientFactory;
            _cache = distributedCache;
            _user = user.Value;
            _logger = logger;
        }       

        public async Task<string> GetClientAccessTokenAsync(bool forceRenewal = false)
        {
            if (forceRenewal)
            {
                _logger.LogInformation("Requested new access token");

                var token = await RequestClientAccessTokenAsync();

                _cache.Remove(BsSessionId);

                _cache.Set(BsSessionId, token);

                return token;
            }

            var cachedToken = _cache.Get<string>(BsSessionId);

            if (string.IsNullOrEmpty(cachedToken))
            {
                _logger.LogInformation("access token not found");

                var token = await RequestClientAccessTokenAsync();

                _cache.Set(BsSessionId, token);

                return token;
            }
            else
            {
                return cachedToken;
            }
        }

        private async Task<string> RequestClientAccessTokenAsync()
        {
            var json = JsonSerializer.Serialize(_user);

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/login");

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpClient = _clientFactory.CreateClient(AccessTokenManagementDefaults.DefaultTokenClientName);

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("access token received");

                var accessToken = response.Headers.GetValues("bs-session-id").First();

                return accessToken;
            }

            return null;
        }
    }
}
