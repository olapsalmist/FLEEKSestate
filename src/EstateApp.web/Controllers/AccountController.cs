using System;
using EstateApp.web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EstateApp.web.Controllers
{
    public class AccountController : Controller
    {

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
    public IActionResult Login (LoginModel model)
    {
        //this statement is used immediately after the required in the model
        if(!ModelState.IsValid) return View();
        throw new NotImplementedException();
    }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if(!ModelState.IsValid) return View(); /*always remember to debug to check if u get desired values back*/
            
        }
    }
}