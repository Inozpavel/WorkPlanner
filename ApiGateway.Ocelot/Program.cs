using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ApiGateway.Ocelot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureLogging(options => { options.AddConsole(); }).ConfigureAppConfiguration(
                    (hostingContext, config) =>
                    {
                        if (hostingContext.HostingEnvironment.IsDevelopment())
                            config.AddJsonFile("ocelot.json", false, true);
                        else if (hostingContext.HostingEnvironment.IsProduction())
                            config.AddJsonFile("ocelot.production.json", false, true);
                    });
    }
}