using HulaQuanOriginal.DAL;
using HulaQuanOriginal.Helpers;
using HulaQuanOriginal.Models;
using HulaQuanOriginal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HulaQuanOriginal.Controllers
{
    [Authorize]
    public class HulaMeController : Controller
    {
        private HulaContext hulaDb = new HulaContext();

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existedUser = hulaDb.Users.FirstOrDefault(u =>
                        u.Name.Equals(model.UserName, StringComparison.InvariantCultureIgnoreCase) ||
                        u.Email.Equals(model.Email, StringComparison.InvariantCultureIgnoreCase));
                if (existedUser == null)
                {
                    var key = StringHelper.GetRandomString(10);
                    var pwdInHash = PasswordHelper.EncodePassword(model.Password, key);
                    var user = new User
                    {
                        Name = model.UserName,
                        Email = model.Email,
                        Password = pwdInHash,
                        Key = key
                    };

                    hulaDb.Users.Add(user);
                    hulaDb.SaveChanges();

                    return RedirectToAction("Login");
                }
                ModelState.AddModelError("", "User already exists!");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var existedUser = hulaDb.Users.SingleOrDefault(u => 
                        u.Email.Equals(model.Email, StringComparison.InvariantCultureIgnoreCase));

                if (existedUser != null)
                {
                    var pwdInHash = PasswordHelper.EncodePassword(model.Password, existedUser.Key);
                    if (pwdInHash == existedUser.Password)
                    {
                        FormsAuthentication.SetAuthCookie(existedUser.Email, false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                ModelState.AddModelError("", "User name or Password not correct!");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Get(int id)
        {
            return View();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}