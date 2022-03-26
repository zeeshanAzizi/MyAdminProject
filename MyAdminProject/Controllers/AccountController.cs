
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MyAdminProject.Repository.Interface;
using MyAdminProject.Utils.Enums;
using MyAdminProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyAdminProject.Controllers
{
    
    public class AccountController : Controller
    {
        private IUsers UserService;
         public AccountController(IUsers users)
        {
            UserService = users;
        }
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                var result = UserService.SignIn(model);
                if (result == SignInEnum.Success)
                {
                    var claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name, model.Email),
                       
                };
                    //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
                    var principal = new ClaimsPrincipal(identity);
                    //SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                    {
                        IsPersistent = model.RememberMe
                    });
                    return RedirectToAction("Index", "Home");
                }
                else if(result==SignInEnum.WrongCredentials)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login credential !");
                }
                else if (result == SignInEnum.NotVerified)
                {
                    ModelState.AddModelError(string.Empty, "User not verified !");
                }
                else if (result == SignInEnum.InActive)
                {
                    ModelState.AddModelError(string.Empty, "Your Account is inactive !");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(SignUpModel model)
        {
            if (ModelState.IsValid)
            {

            }
          return View();
        }
    }
}
