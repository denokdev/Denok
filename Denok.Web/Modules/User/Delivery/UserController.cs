using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Denok.Web.Modules.User.Delivery
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        private readonly Usecase.IUserUsecase _userUsecase;

        private readonly Modules.Link.Usecase.ILinkUsecase _linkUsecase;

        public UserController(ILogger<UserController> logger, 
            Usecase.IUserUsecase userUsecase, Modules.Link.Usecase.ILinkUsecase linkUsecase)
        {
            _logger = logger;
            _userUsecase = userUsecase;
            _linkUsecase = linkUsecase;
        }

        public async Task<IActionResult> Dashboard(Modules.Link.Model.GenerateViewModel viewModel)
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
            if (viewModel == null) 
            {
                viewModel = new Modules.Link.Model.GenerateViewModel(); 
            }

            viewModel.Username = user.Username;
            
            return View(viewModel);
        }

        public async Task<IActionResult> Links(int limit, int page)
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
            var viewModel = new Modules.Link.Model.GenerateViewModel();
            viewModel.Username = user.Username;

            var linkFilter = new Modules.Link.Model.LinkFilter();
            if (limit > 0)
            {
                Console.WriteLine(String.Format("Limit: {0}", limit));
                linkFilter.Limit = (uint) limit;
            }

            if (page > 0)
            {
                Console.WriteLine(String.Format("Page: {0}", page));
                linkFilter.Page = (uint) page;
            }

            var getLinksResult = await _linkUsecase.GetLinks(linkFilter);
            if (getLinksResult.IsError())
            {
                viewModel.ErrorMessage = "error fetch link data";
                return View(viewModel);
            }

            var linkListView = getLinksResult.Get();
            viewModel.LinkListView = linkListView;
            
            return View(viewModel);
        }

        public async Task<IActionResult> Users()
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
            var viewModel = new Modules.Link.Model.GenerateViewModel();
            viewModel.Username = user.Username;
            
            return View(viewModel);
        }

        public async Task<IActionResult> Profile()
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
            var viewModel = new Modules.Link.Model.GenerateViewModel();
            viewModel.Username = user.Username;
            viewModel.Profile = user;
            
            return View(viewModel);
        }
    }
}