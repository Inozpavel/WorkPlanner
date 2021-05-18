using IdentityServer.Data;
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
                .AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationContext>();

            services.AddIdentityServer(options => { options.IssuerUri = _configuration["IssuerUri"]; })
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.GetConfiguredClients(_configuration))
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddAspNetIdentity<User>()
                .AddProfileService<ProfileService>()
                .AddDeveloperSigningCredential();

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