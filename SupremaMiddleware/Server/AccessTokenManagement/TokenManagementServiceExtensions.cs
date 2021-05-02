using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;

namespace SupremaMiddleware.Server
{
    public static class TokenManagementServiceExtensions
    {
        /// <summary>
        /// Adds the token management services to DI
        /// </summary>
        public static void AddAccessTokenManagement(this IServiceCollection services,
            Action<HttpClient> configureClient = null)
        {
            if (configureClient != null)
            {
                services.AddHttpClient("TokenService", configureClient);
            }

            services.AddMemoryCache();

            services.TryAddTransient<IClientAccessTokenManagementService, ClientAccessTokenManagementService>();

            // The DelegatingHandler has to be registered as a Transient Service
            services.TryAddTransient<ClientAccessTokenHandler>();
        }

        /// <summary>
        /// Adds the client access token handler to an HttpClient
        /// </summary>
        public static IHttpClientBuilder AddClientAccessTokenHandler(this IHttpClientBuilder httpClientBuilder)
        {
            return httpClientBuilder.AddHttpMessageHandler<ClientAccessTokenHandler>();
        }
    }
}
