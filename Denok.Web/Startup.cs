using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

            // add middleware
            app.UseWhen(context => {
                // return Regex.Match(context.Request.Path.Value, @"\/api\/\w+").Success;
                return context.Request.Path.StartsWithSegments("/api");
            }, appBuilder => {
                appBuilder.UseMiddleware<Middleware.BasicAuthMiddleware>();
                appBuilder.UseStatusCodePages(Middleware.CustomErrorMiddleware.Handle404);
                appBuilder.Use(async (context, next) =>
                {
                    await next();
                    if (context.Response.StatusCode == 404)
                    {
                        context.Request.Path = "/api/not-found";
                        await next();
                    }
                });
            });

             app.UseWhen(context => {
                return !context.Request.Path.StartsWithSegments("/api");
            }, appBuilder => {
                 // handler error such as method not allowed, and etc
                appBuilder.UseStatusCodePagesWithRedirects("/Error/PageNotFound");

                appBuilder.Use(async (context, next) =>
                {
                    await next();
                    if (context.Response.StatusCode == 404)
                    {
                        context.Request.Path = "/Error/PageNotFound";
                        await next();
                    }
                });
            });

            // app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/api", IndexApi);
                endpoints.MapGet("/api/not-found", NotFoundApi);
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static async Task IndexApi(HttpContext context)
        {
            await context.Response.WriteAsJsonAsync(
                new Denok.Lib.Shared.Response<Lib.Shared.EmptyJson> (
                    success: true,
                    code: 200,
                    message: "service up and running",
                    data: new Lib.Shared.EmptyJson()
                )
            );
        }

        private static async Task NotFoundApi(HttpContext context)
        {
            await context.Response.WriteAsJsonAsync(
                new Denok.Lib.Shared.Response<Lib.Shared.EmptyJson> (
                    success: false,
                    code: 404,
                    message: "resource not found",
                    data: new Lib.Shared.EmptyJson()
                )
            );
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
