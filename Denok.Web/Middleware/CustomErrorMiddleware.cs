using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Denok.Web.Middleware {

    public  static class CustomErrorMiddleware 
    {

        public static async Task Handle404(StatusCodeContext context)
        {
            context.HttpContext.Response.StatusCode = 404;
            await context.HttpContext.Response.WriteAsJsonAsync(
                new Denok.Lib.Shared.Response<Denok.Lib.Shared.EmptyJson> (
                    success: false,
                    code: 404,
                    message: "resources not found",
                    data: new Denok.Lib.Shared.EmptyJson{}
                )
            );
        }
    }

    
}