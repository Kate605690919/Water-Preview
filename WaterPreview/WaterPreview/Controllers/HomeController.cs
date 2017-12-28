using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterPreview.Base;
using WaterPreview.Other;
using WaterPreview.Other.Attribute;
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
        public ActionResult Login(string userName, string password,string backUrl)
        {
            User_t user = accountService.GetAccountByName(userName);
            if (user.Usr_UId == new Guid() || user.Usr_Password != MD5_Util.MD5Encrypt(password))
            {
                ViewBag.Exception = true;
                return View();
            }

            Response.Cookies[ConfigurationManager.AppSettings["CookieName"]].Value = user.Usr_UId.ToString();
            Response.Cookies[ConfigurationManager.AppSettings["CookieName"]].Domain = ConfigurationManager.AppSettings["DomainName"];
            Response.Cookies[ConfigurationManager.AppSettings["CookieName"]].Expires = DateTime.Now.AddDays(1); 

            //Session["CookieName"] = user.Usr_UId.ToString();
            //Session.Timeout = 1440;
            //TempData["Cookie"] = user.Usr_UId.ToString();
            //TokenIds.Add(Session.SessionID, Guid.NewGuid());
            return RedirectToAction("index",false);
            //return View("index");
        }       

        public ActionResult Index()
        {
            //string uid = TempData["Cookie"].ToString();
            //Cookie cookie = new Cookie();
            
            return View();
        }

        public ActionResult LogOut()
        {
            User_t account = UserContext.account;
            account = account != null && account.Usr_UId != new Guid() ? account : new User_t();
            HttpCookie cookie = Request.Cookies[ConfigurationManager.AppSettings["CookieName"]];
            if (cookie != null)
            {
                cookie.Domain = ConfigurationManager.AppSettings["DomainName"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Login", "Home");
        }
    }
}