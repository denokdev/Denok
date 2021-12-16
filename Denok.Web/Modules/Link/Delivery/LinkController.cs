using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Denok.Web.Modules.Link.Delivery
{
    public class LinkController : Controller
    {
        private readonly ILogger<LinkController> _logger;

        private readonly Modules.User.Usecase.IUserUsecase _userUsecase;

        private readonly Modules.Link.Usecase.ILinkUsecase _linkUsecase;

        public LinkController(ILogger<LinkController> logger, 
            Modules.User.Usecase.IUserUsecase userUsecase, Modules.Link.Usecase.ILinkUsecase linkUsecase)
        {
            _logger = logger;
            _userUsecase = userUsecase;
            _linkUsecase = linkUsecase;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Generate([Bind("OriginalLink,Description")] Modules.Link.Model.GenerateRequest generateRequest)
        {
            if (HttpContext.Session.GetString(Utils.Constants.UserIdSessionKey) == null)
            {
                return RedirectToAction("Index", "Home");  
            }

            var userId = HttpContext.Session.GetString(Utils.Constants.UserIdSessionKey);
            var findByIdResult = await _userUsecase.GetProfile(userId);
            if (findByIdResult.IsError())
            {
                return RedirectToAction("Index", "Home");
            }

            var user = findByIdResult.Get();
            var viewModel = new Model.GenerateViewModel();
            viewModel.Username = user.Username;

            if (ModelState.IsValid) 
            {
                Uri uriResult;
                bool validUrl = Utils.Utils.ValidHttpURL(generateRequest.OriginalLink, out uriResult);

                if (!validUrl)
                {
                    viewModel.ErrorMessage = "invalid url";
                    return RedirectToAction("Dashboard", "User", viewModel);
                }

                var domainName = Config.AppConfig.DomainName;
                var outputLink = Lib.LinkGenerator.LinkGenerator.Generate();
                viewModel.GeneratedLink = String.Format("{0}/{1}", domainName, outputLink);

                // save
                var linkRequest = new Model.LinkRequest();
                linkRequest.CreatedBy = user.Username;
                linkRequest.OriginalLink = uriResult.AbsoluteUri;
                linkRequest.GeneratedLink = outputLink;
                if (generateRequest.Description != null)
                {
                    linkRequest.Description = generateRequest.Description;
                }
                linkRequest.TotalVisits = 0;

                var saveLinkResult = await _linkUsecase.CreateLink(linkRequest);
                if (saveLinkResult.IsError()) 
                {
                    viewModel.ErrorMessage = "error save generated link";
                    return RedirectToAction("Dashboard", "User", viewModel);
                }
                
                return RedirectToAction("Dashboard", "User", viewModel);
            }
            
            return RedirectToAction("Dashboard", "User", viewModel);
        }
    }
}