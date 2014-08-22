using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using Weitm.Modules.Membership;
using Weitm.Modules.Membership.Models;

namespace Weitm.Applications.ChainTouch.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Weitm.Modules.Membership.Models.RegisterModel model, string returnUrl)
        {
            model.UserName = model.Email;

            ModelState.Clear();
            TryValidateModel(model);

            if (ModelState.IsValid)
            {
                if (WebSecurity.UserExists(model.UserName))
                {
                    ModelState.AddModelError("", "The account is registered.");
                }
                else
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password,
                        new
                        {
                            Email = model.Email,
                            NickName = model.Email.Split('@')[0],
                            UserName = model.Email
                        });
                    Roles.AddUserToRole(model.UserName, "Users");
                    Guid userId = WebSecurity.GetUserId(model.UserName);
                    WebSecurity.Login(model.UserName, model.Password);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return Redirect("~/");
                    }
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/");
        }

        [AllowAnonymous]
        public ActionResult Register(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            ViewBag.Message = "";
            if (ModelState.IsValid)
            {
                if (Weitm.Modules.Membership.Models.Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return Redirect("~/");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The provided password is not correct.");
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        public ActionResult ChangePassword()
        {
            if (Request.Form.Count > 0)
            {
                string oldPassword = Request.Form["OldPassword"].ToString();
                string newPassword = Request.Form["NewPassword"].ToString();
                string confirmPassword = Request.Form["ConfirmPassword"].ToString();

                if (newPassword == confirmPassword && WebSecurity.ChangePassword(User.Identity.Name, oldPassword, newPassword))
                {
                    ViewBag.Message = "Password changed succcessfully !";
                }
                else
                {
                    ViewBag.Message = "Sorry, fail to change passsword !";
                }

            }
            return View();
        }
    }
}
