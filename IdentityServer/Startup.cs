using IdentityServer.Data;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Users.Data.Entities;

namespace IdentityServer
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
                {
                    options.UseNpgsql(_configuration.GetConnectionString("Identity"));
                })
                .AddIdentity<User, Role>().AddEntityFrameworkStores<ApplicationContext>();

            services.AddIdentityServer(options => { options.IssuerUri = _configuration["IssuerUri"]; })
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.GetConfiguredClients(_configuration))
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddProfileService<ProfileService>()
                .AddAspNetIdentity<User>()
                .AddDeveloperSigningCredential();

            services.AddScoped<IProfileService, ProfileService>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}