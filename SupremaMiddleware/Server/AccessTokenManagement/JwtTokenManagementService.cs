using ConsoleApp1.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SupremaMiddleware.Server.AccessTokenManagement;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SupremaMiddleware.Server.AccessTokenManagement
{
    public class JwtTokenManagementService : IClientAccessTokenManagementService
    {
        private static Timer aTimer;

        private const string JwtToken = "JwtToken";
        private const string RefreshToken = "RefreshToken"; 

        private readonly IHttpClientFactory _clientFactory;
        private readonly IMemoryCache _cache;
        private readonly User _user;
        private readonly ILogger<JwtTokenManagementService> _logger;
        private readonly ClientTokenRequestParameters _tokenRequestParameters;

        public JwtTokenManagementService(IHttpClientFactory clientFactory,
            IMemoryCache memoryCache,
            IOptions<User> user,
            ILogger<JwtTokenManagementService> logger,
            ClientTokenRequestParameters tokenRequestParameters)
        {
            _clientFactory = clientFactory;
            _cache = memoryCache;             
            _user = user.Value;
            _logger = logger;
            _tokenRequestParameters = tokenRequestParameters;
        }

        public async Task<string> GetClientAccessTokenAsync(bool forceRenewal = false)
        {
            var cachedToken = _cache.Get<string>(JwtToken);

            if (string.IsNullOrEmpty(cachedToken))
            {
                _logger.LogInformation("Jwt token not found");

                var jwtToken = await RequestClientAccessTokenAsync();       

                var timeGap = jwtToken.ValidTo - DateTime.Now;

                SetTimer(timeGap.TotalSeconds);

                _cache.Set(JwtToken, jwtToken);

                return jwtToken.;
            }
            else
            {
                return Task.FromResult(cachedToken);
            }

            return Task.FromResult("");
        }

        private Task<SecurityToken> RequestClientAccessTokenAsync()
        {
            //var json = JsonSerializer.Serialize(_user);

            //var request = new HttpRequestMessage(HttpMethod.Post, _tokenRequestParameters.TokenRequestUri);

            //request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            //var httpClient = _clientFactory.CreateClient(AccessTokenManagementDefaults.DefaultTokenClientName);

            //var response = await httpClient.SendAsync(request);

            //if (response.IsSuccessStatusCode)
            //{
            //    _logger.LogInformation("access token received");

            //    var accessToken = response.Headers.GetValues(_tokenRequestParameters.AuthorizationHeaderName).First();

            //    return accessToken;
            //}

            // Get new one
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJBRE1JTiIsImp0aSI6IjY5ODhmNzA3LTQ5ODUtNDRiYS04MDg0LTJlOWIyZTA0OTg5MSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiMSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJBRE1JTiIsIlBlcm1pc3Npb24iOlsidXNlcnMud3JpdGUiLCJyZXBvcnRzLnJlYWQiLCJsb2NhdGlvbnMud3JpdGUiLCJsb2NhdGlvbnMucmVhZCIsIm1hcC5yZWFkIiwibWFwLndyaXRlIiwic2NyaXB0cy53cml0ZSIsInNldHRpbmdzLnJlYWQiLCJzZXR0aW5ncy53cml0ZSIsImZpbHRlcnMucmVhZCIsImV2ZW50cy5yZWFkIiwiZG9vcnMucmVhZCIsImRldmljZXMud3JpdGUiLCJkZXZpY2VzLnJlYWQiLCJvcGVyYXRvcnMucmVhZCIsIm9wZXJhdG9ycy53cml0ZSIsInVzZXJzLnJlYWQiLCJzY2hlZHVsZXMucmVhZCIsInNjaGVkdWxlcy53cml0ZSIsImRvb3JzLndyaXRlIiwiZmlsdGVycy53cml0ZSIsImFwYi5yZWFkIiwiYXBiLndyaXRlIl0sImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlN5c3RlbSBhZG1pbmlzdHJhdG9yIiwibmJmIjoxNjIwNTUyMDk1LCJleHAiOjE2MjIwODgwOTUsImlzcyI6Imh0dHBzOi8vbWlkcG9pbnQtc2VjdXJpdHkuY29tIiwiYXVkIjoiaHR0cHM6Ly9taWRwb2ludC1zZWN1cml0eS5jb20ifQ.v49jsmBdaorjIXNJnsNK1vpUmJcrTA_EKtMsvRK7g9o";

            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadToken(token);

            return Task.FromResult(jwtSecurityToken);
        }

        private static void SetTimer(double interval)
        {
            // Create a timer with a two second interval.
            aTimer = new Timer(interval);

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                              e.SignalTime);
        }
    }
}
