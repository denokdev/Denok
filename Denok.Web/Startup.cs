using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Denok.Web.Config;

namespace Denok.Web
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
            RegisterController(services);
            services.AddControllersWithViews();
            
            services.AddDistributedMemoryCache();
            services.AddSession();
            
            // register mongodb
            RegisterMongoDb(services);

            // register user module
            RegisterUserModule(services);

            // register link module
            RegisterLinkModule(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // handler error such as method not allowed, and etc
            app.UseStatusCodePagesWithRedirects("/Error/PageNotFound");

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Error/PageNotFound";
                    await next();
                }
            });

            // app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        // register API Controller
        private static void RegisterController(IServiceCollection services)
        {
            services.AddControllers()
            .ConfigureApiBehaviorOptions(options => {
                // custom error response for invalid ModelState
                options.InvalidModelStateResponseFactory = Utils.Utils.CustomInvalidStateModelFactory;
            });
        }

        private static void RegisterMongoDb(IServiceCollection services)
        {
            services.AddSingleton<Lib.Database.Mongo.IMongoConfig>(x => {
                return new Lib.Database.Mongo.MongoConfig() {
                    ConnectionString = AppConfig.MongoDbConnectionWrite,
                    DatabaseName = AppConfig.MongoDbNameWrite
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

        private static void RegisterUserModule(IServiceCollection services)
        {
            services.AddSingleton<Modules.User.Repository.IUserRepository, Modules.User.Repository.UserRepositoryMongo>();
            services.AddSingleton<Modules.User.Usecase.IUserUsecase, Modules.User.Usecase.UserUsecase>();
        }

        private static void RegisterLinkModule(IServiceCollection services)
        {
            services.AddSingleton<Modules.Link.Repository.ILinkRepository, Modules.Link.Repository.LinkRepositoryMongo>();
            services.AddSingleton<Modules.Link.Usecase.ILinkUsecase, Modules.Link.Usecase.LinkUsecase>();
        }
    }
}
