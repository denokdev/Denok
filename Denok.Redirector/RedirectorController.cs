using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Denok.Redirector.Controller
{
    [ApiController]
    [Route("")]
    public class RedirectController : ControllerBase
    {
        // redirect to this domain
        // if generated link not founnd
        private readonly string domainNotFound = Denok.Web.Config.AppConfig.DomainNotFound;

        private readonly ILogger<RedirectController> _logger;
        private readonly Denok.Web.Modules.Link.Usecase.ILinkUsecase _linkUsecase;

        public RedirectController(ILogger<RedirectController> logger, 
            Denok.Web.Modules.Link.Usecase.ILinkUsecase linkUsecase)
        {
            _logger = logger;
            _linkUsecase = linkUsecase; 
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> Process(string code)
        {
            var findByCodeResult = await _linkUsecase.GetLinkByGeneratedLink(code);
            if (findByCodeResult.IsError())
            {
                return RedirectPermanent(domainNotFound);
            }

            var link = findByCodeResult.Get();
            var redirectTo = link.OriginalLink;

            return RedirectPermanent(redirectTo);
        }
    }
}