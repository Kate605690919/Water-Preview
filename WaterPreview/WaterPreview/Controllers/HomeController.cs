using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterPreview.Base;
using WaterPreview.Other;
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
        public ActionResult Login(string userName, string password)
        {
            User_t user = accountService.GetAccountByName(userName);
            if (user.Usr_UId == new Guid() || user.Usr_Password != MD5_Util.MD5Encrypt(password))
            {
                ViewBag.Exception = true;
                return View();
            }
            Response.Cookies["username"].Value = user.Usr_UId.ToString();
            Response.Cookies["username"].Expires = DateTime.Now.AddDays(1);
            return RedirectToAction("index");
        }

        public JsonResult GetMostVisitsDevice(string[] uidlist)
        {
            JsonResult result = new JsonResult();
            return result;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}