using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Denok.Web.Models;

namespace Denok.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Modules.User.Usecase.IUserUsecase _userUsecase;

        public HomeController(ILogger<HomeController> logger, Modules.User.Usecase.IUserUsecase userUsecase)
        {
            _logger = logger;
            _userUsecase = userUsecase;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username,Password")] Modules.User.Model.LoginRequest loginRequest)
        {
            if (ModelState.IsValid)
            {
                var loginResult = await _userUsecase.Login(loginRequest);
                if (loginResult.IsError())
                {
                    ViewBag.ErrorMessage = loginResult.Error();
                    return View(nameof(Index),loginRequest);
                }

                var user = loginResult.Get();
                Console.WriteLine(user.Username);
                HttpContext.Session.SetString(Utils.Constants.UserIdSessionKey, user.Id);

                return RedirectToAction("Dashboard", "User");
            }

            return View(nameof(Index),loginRequest);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString(Utils.Constants.UserIdSessionKey) == null)
            {
                return RedirectToAction("Index", "Home");  
            }

            HttpContext.Session.Remove(Utils.Constants.UserIdSessionKey);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
