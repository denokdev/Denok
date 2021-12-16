using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Denok.Web.Config;

namespace Denok.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                AppConfig.Init();
            } catch (InvalidConfigException ex)
            {
                Console.WriteLine($"{ex.Message}");
                Environment.Exit(1);
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // configure app config
                    webBuilder.ConfigureAppConfiguration((context, config) => {
                        config.AddEnvironmentVariables();
                    });

                    // configure web server
                    // webBuilder.ConfigureKestrel(options => {
                    //     options.Listen(System.Net.IPAddress.Loopback, PORT);
                    // });

                    webBuilder.UseKestrel();

                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls(String.Format($"http://0.0.0.0:{AppConfig.HttpPortWeb}"));
                });
    }
}
