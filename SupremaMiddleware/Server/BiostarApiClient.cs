using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SupremaMiddleware.Server.Models;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SupremaMiddleware.Server
{
    public class BiostarApiClient : IBiostarApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BiostarApiClient> _logger;

        public BiostarApiClient(HttpClient httpClient, 
            ILogger<BiostarApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<UserCollection> GetUserCollection(int? offset = 1, int? limit = 100)
        {
            var cancellation = new CancellationTokenSource();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/users?limit={limit}&offset={offset}");

            var response = await _httpClient.SendAsync(request, cancellation.Token);

            var content = await response.Content.ReadAsStringAsync();

            var root = JsonSerializer.Deserialize<Root>(content);

            return root.UserCollection;
        }
    }
}
