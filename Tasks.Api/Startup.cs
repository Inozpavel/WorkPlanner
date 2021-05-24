using System;
using System.Collections.Generic;
using System.Reflection;
using IdentityServer4.AccessTokenValidation;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Tasks.Api.Data;
using Tasks.Api.MassTransit;
using Tasks.Api.Services;

namespace Tasks.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseNpgsql(_configuration.GetConnectionString("TasksDb") + _configuration["DbName"]);
            });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = _configuration["IdentityServer:Authority"];
                    options.RequireHttpsMetadata = false;
                });

            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "RoomsApi",
                    Description = "Service for managing rooms and tasks in rooms",
                    Version = "v1",

                    Contact = new OpenApiContact
                    {
                        Email = "inozpavel@mail.ru",
                        Name = "Pavel Inozemtsev"
                    }
                });

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,

                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri(_configuration["IdentityServer:TokenUrl"])
                        }
                    }
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<UnitOfWork>();

            services.AddScoped<UserService>();
            services.AddScoped<RoomService>();
            services.AddScoped<RoomTaskService>();
            services.AddScoped<RoleService>();

            services.AddTransient<DatabaseInitializer>();

            services.AddControllers();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserRegisteredConsumer>();
                x.AddConsumer<ProfileUpdatedConsumer>();

                x.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(_configuration["MassTransit:Host"],
                        _configuration["MassTransit:VirtualHost"], options =>
                        {
                            options.Username(_configuration["MassTransit:Username"]);
                            options.Password(_configuration["MassTransit:Password"]);
                        });

                    configurator.ReceiveEndpoint("registered-users",
                        e => { e.ConfigureConsumer<UserRegisteredConsumer>(context); });

                    configurator.ReceiveEndpoint("updated-profiles",
                        e => { e.ConfigureConsumer<ProfileUpdatedConsumer>(context); });
                });
            });


            services.AddMassTransitHostedService();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseInitializer databaseInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            databaseInitializer.Initialize();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "TasksApi");

                options.DocumentTitle = "TasksApi";

                options.OAuthClientId(_configuration["IdentityServer:ClientId"]);
                options.OAuthClientSecret(_configuration["IdentityServer:Secret"]);
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}