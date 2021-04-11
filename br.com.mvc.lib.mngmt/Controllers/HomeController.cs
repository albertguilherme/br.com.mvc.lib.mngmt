using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using br.com.mvc.lib.mngmt.repository;
using br.com.mvc.lib.mngmt.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace br.com.mvc.lib.mngmt.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MNGMTContext _context;

        public HomeController(ILogger<HomeController> logger, MNGMTContext context)
        {
            _logger = logger;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Index([Bind("Username,Password,ReturnUrl")] LoginUserViewModel user)
        {
            var validUser = await new bizrules.User().AuthenticateUser(user.Username, user.Password);

            if (validUser != null)
            {
                var claims = new List<Claim>
                {
                    new ("username", validUser.Username),
                    new (ClaimTypes.Name, validUser.Name)
                };
                claims.AddRange(validUser.Roles?.Split(',').Select(role => new Claim(ClaimTypes.Role, role)));

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true
                    });

                if(!string.IsNullOrEmpty(user.ReturnUrl))
                    return Redirect(user.ReturnUrl);

                return RedirectToAction("Index", "Books");
            }
            return View(user);
        }

        [HttpPost, ActionName("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index");
        }

    }
}
