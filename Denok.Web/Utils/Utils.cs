using System;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Denok.Web.Utils
{

    public static class Utils
    {
        public static IActionResult CustomInvalidStateModelFactory(ActionContext context)
        {
            return new BadRequestObjectResult(
                new Denok.Lib.Shared.Response<Lib.Shared.EmptyJson> (
                    success: false,
                    code: 400,
                    message: "invalid input body",
                    data: new Lib.Shared.EmptyJson()
                )
            );
        }

        public static bool ValidHttpURL(string s, out Uri resultURI)
        {
            if (!Regex.IsMatch(s, @"^https?:\/\/", RegexOptions.IgnoreCase))
                s = "http://" + s;

            if (Uri.TryCreate(s, UriKind.Absolute, out resultURI))
                return (resultURI.Scheme == Uri.UriSchemeHttp || 
                        resultURI.Scheme == Uri.UriSchemeHttps);

            return false;
        }
    }
}