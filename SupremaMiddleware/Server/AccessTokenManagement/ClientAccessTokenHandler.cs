using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SupremaMiddleware.Server
{
    public class ClientAccessTokenHandler : DelegatingHandler
    {
        private readonly IClientAccessTokenManagementService _accessTokenManagementService;

        public ClientAccessTokenHandler(IClientAccessTokenManagementService accessTokenManagementService)
        {
            _accessTokenManagementService = accessTokenManagementService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await SetAccessTokenAsync(request);

            var response = await base.SendAsync(request, cancellationToken);

            // retry if 401
            if (response.StatusCode is System.Net.HttpStatusCode.Unauthorized)
            {
                response.Dispose();

                await SetAccessTokenAsync(request, true);

                return await base.SendAsync(request, cancellationToken);
            }

            return response;
        }

        public async Task SetAccessTokenAsync(HttpRequestMessage request, bool forceRenewal = false)
        {
            var accessToken = await _accessTokenManagementService.GetClientAccessTokenAsync();

            request.Headers.Add("bs-session-id", accessToken);
        }
    }
}
