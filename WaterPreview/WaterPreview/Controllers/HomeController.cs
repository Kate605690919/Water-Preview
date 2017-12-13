using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterPreview.Base;
using WaterPreview.Other;
using WaterPreview.Redis;
using WaterPreview.Service;
using WaterPreview.Service.Interface;

namespace WaterPreview.Controllers
{
    public class HomeController : BaseController
    {

        private static IAccountService accountService;

        public HomeController(IAccountService accService)
        {
            this.AddDisposableObject(accService);
            accountService = accService;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewBag.Exception = false;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string userName, string password)
        {
            User_t user = accountService.GetAccountByName(userName);
            if (user.Usr_UId == new Guid() || user.Usr_Password != MD5_Util.MD5Encrypt(password))
            {
                ViewBag.Exception = true;
                return View();
            }
            Response.Cookies["wp_username"].Value = user.Usr_UId.ToString();
            Response.Cookies["wp_username"].Expires = DateTime.Now.AddDays(1);

            return RedirectToAction("index");
        }       

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            User_t account = UserContext.account;
            if (account != null && account.Usr_UId != new Guid())
            {
                UserContext.account = new User_t();
            }
            HttpCookie cookies = Request.Cookies["wp_username"];
            if (cookies != null)
            {
                cookies.Expires = DateTime.Now.AddDays(-1);
                System.Web.HttpContext.Current.Request.Cookies.Add(cookies);
            }

            return RedirectToAction("Login", "Home");
        }
    }
}