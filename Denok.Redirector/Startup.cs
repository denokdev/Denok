using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Denok.Redirector
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Denok.Redirector", Version = "v1" });
            });

            // register mongodb
            RegisterMongoDb(services);

            // register link module
            RegisterLinkModule(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Denok.Redirector v1"));
            }

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    var redirectTo = Denok.Web.Config.AppConfig.DomainNotFound;
                    context.Response.Redirect(redirectTo);
                    return;
                }
            });

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void RegisterMongoDb(IServiceCollection services)
        {
            services.AddSingleton<Lib.Database.Mongo.IMongoConfig>(x => {
                return new Lib.Database.Mongo.MongoConfig() {
                    ConnectionString = Denok.Web.Config.AppConfig.MongoDbConnectionWrite,
                    DatabaseName = Denok.Web.Config.AppConfig.MongoDbNameWrite
                };
            });

            services.AddSingleton<Lib.Database.Mongo.IMongo>(x => {
                var mongoWrite = new Lib.Database.Mongo.Mongo(
                    x.GetRequiredService<ILogger<Lib.Database.Mongo.Mongo>>(),
                    x.GetRequiredService<Lib.Database.Mongo.IMongoConfig>()
                );

                mongoWrite.Connect();

                return mongoWrite;
            });
        }

        private static void RegisterLinkModule(IServiceCollection services)
        {
            services.AddSingleton<Denok.Web.Modules.Link.Repository.ILinkRepository, Denok.Web.Modules.Link.Repository.LinkRopositoryMongo>();
            services.AddSingleton<Denok.Web.Modules.Link.Usecase.ILinkUsecase, Denok.Web.Modules.Link.Usecase.LinkUsecase>();
        }
    }
}
