using System.Reflection;
using IdentityServer.Data;
using IdentityServer.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
                .AddIdentity<User, Role>(config =>
                {
                    config.Password.RequiredLength = 6;
                    config.User.RequireUniqueEmail = true;
                    config.Password.RequireDigit = true;
                    config.Password.RequireUppercase = false;
                }).AddEntityFrameworkStores<ApplicationContext>();

            services.AddIdentityServer(options => { options.IssuerUri = _configuration["IssuerUri"]; })
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.GetConfiguredClients(_configuration))
                .AddAspNetIdentity<User>()
                .AddDeveloperSigningCredential();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "IdentityApi",
                    Version = "v1",
                    Description = "IdentityApi"
                });
            });

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddTransient<DatabaseInitializer>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseInitializer initializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            initializer.Initialize();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityApi");
            });

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}