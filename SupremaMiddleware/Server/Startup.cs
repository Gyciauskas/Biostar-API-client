using ConsoleApp1.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SupremaMiddleware.Server.AccessTokenManagement;
using System;

namespace SupremaMiddleware.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var biostarUrl = new Uri(Configuration.GetValue<string>("BiostarUrl"));

            services.Configure<User>(Configuration.GetSection(nameof(User)));

            services.AddAccessTokenManagement(new ClientTokenRequestParameters {
                TokenRequestUri = "/api/login",
                AuthorizationHeaderName = "bs-session-id"
            }, c =>
            {
                c.BaseAddress = biostarUrl;
            });

            services.AddHttpClient<IBiostarApiClient, BiostarApiClient>(c =>
            {
                c.BaseAddress = biostarUrl;
            })
            .AddClientAccessTokenHandler();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
