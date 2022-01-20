using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Denok.Web.Middleware 
{

    public class BasicAuthMiddleware 
    {
        private readonly ILogger<BasicAuthMiddleware> _logger;
        private readonly RequestDelegate _next;

        public BasicAuthMiddleware(RequestDelegate next, ILogger<BasicAuthMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            bool isAuthorizationPresent = context.Request.Headers.TryGetValue("Authorization", out var authorization);
            if (!isAuthorizationPresent) 
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(
                    new Denok.Lib.Shared.Response<Denok.Lib.Shared.EmptyJson> (
                        success: false,
                        code: 401,
                        message: "required authorization",
                        data: new Denok.Lib.Shared.EmptyJson{}
                    )
                );
                return;
            }

            var authorizationSplit = authorization.FirstOrDefault().Split(" ");
            if (authorizationSplit.Count() < 2)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(
                    new Denok.Lib.Shared.Response<Denok.Lib.Shared.EmptyJson> (
                        success: false,
                        code: 401,
                        message: "invalid authorization format",
                        data: new Denok.Lib.Shared.EmptyJson{}
                    )
                );
                return;
            }

            if (!authorizationSplit.First().Equals("Basic"))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(
                    new Denok.Lib.Shared.Response<Denok.Lib.Shared.EmptyJson> (
                        success: false,
                        code: 401,
                        message: "invalid authorization format",
                        data: new Denok.Lib.Shared.EmptyJson{}
                    )
                );
                return;
            }

            var base64Token = authorizationSplit.Last();

            // Decode from Base64 to string
            var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(base64Token));

            // Split username and password
            var username = decodedUsernamePassword.Split(':', 2)[0];
            var password = decodedUsernamePassword.Split(':', 2)[1];

            if (!IsValidAuth(username, password))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(
                    new Denok.Lib.Shared.Response<Denok.Lib.Shared.EmptyJson> (
                        success: false,
                        code: 401,
                        message: "invalid authorization",
                        data: new Denok.Lib.Shared.EmptyJson{}
                    )
                );
                return;
            }

            await _next(context);
        }

        private bool IsValidAuth(string username, string password)
        {
            return username.Equals(Config.AppConfig.BasicAuthUsername) 
                && password.Equals(Config.AppConfig.BasicAuthPassword);
        }
    }
}