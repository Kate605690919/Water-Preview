using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterPreview.Base;
using WaterPreview.Service.Interface;
using WaterPreview.Service.Service;

namespace WaterPreview.Controllers
{
    public class ExceptionController : BaseController
    {
        private static IExceptionService exceptionService;


        public ExceptionController()
        {
            exceptionService = new ExceptionService();
        }
        //public ExceptionController(IExceptionService exService)
        //{
        //    this.AddDisposableObject(exService);
        //    exceptionService = exService;
        //}
        // GET: Exception
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetException(Guid userUid)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = exceptionService.GetException(userUid);
            return result;
        }
    }
}